using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoverController : MonoBehaviour {
	
	public delegate void TaskFinished(BoxController lastBox);
	public event TaskFinished OnTaskFinished;

	public enum Task {
		NONE, //idle
		MOVE, //Just move to target box
		GRAB, //grub stones from the target box
		PUT,  //put one stone to the target box
		PUT_ALL, //put all stone to the target box
		GRAB_OPPONENT //grab stones from opponent
	}

	public float speed = 1.0f;
	public float animSpeed = 1.0f;
	public float boxOffsetX;

	Animator animator;

	BoxController roverBox, targetBox;
	Task task;

	public BoxController RoverBox {
		get { return roverBox; }
	}

	//private 
	public bool isAnimating;
	AudioSource armSound;

	void Awake() {
		armSound = transform.Find("Arm").GetComponent<AudioSource>();
		roverBox = transform.Find("Box").GetComponent<BoxController>();
		animator = GetComponent<Animator>();	
		targetBox = null;
		task = Task.NONE;
		isAnimating = false;
	}
	
	void Update () {
		if ( isAnimating ) {
			// Поки відбувається анімація то чекаємо її завершення
			return;
		}

		switch(task) {
			case Task.GRAB : 
			{
				float destX = targetBox.transform.position.x - boxOffsetX;
				if ( IsReachedTo(destX) ) {
					TriggerAnim("Grab");
				}
			}
			break;

			case Task.PUT_ALL :
			case Task.PUT :
			{
				float destX = targetBox.transform.position.x - boxOffsetX;
				if ( IsReachedTo(destX) ) {
					if (targetBox.isKalah) {
						TriggerAnim("PutKalah");
					} else {
						TriggerAnim("Put");
					}
				}
			}
			break;

			case Task.MOVE : 
			{
				float destX = targetBox.transform.position.x - boxOffsetX;
				if ( IsReachedTo(destX) ) {
					FinishTask(); //Task.MOVE
				}
			}
			break;

			case Task.GRAB_OPPONENT:
			{
				float destX = targetBox.transform.position.x - boxOffsetX;
				if ( IsReachedTo(destX) ) {
					TriggerAnim("GrabOpponent");
				}
			}
			break;
		}
	}

	void TriggerAnim(string triggerName) {
		isAnimating = true;
		animator.SetTrigger(triggerName);
		//Log("Anim'" + triggerName + "'", targetBox);
		armSound.Play();
	}

	bool IsReachedTo(float destX) {
		Vector3 pos = transform.position;
		float dx = speed * Time.deltaTime;

		if ( Mathf.Abs(pos.x - destX) < dx) {
			pos.x = destX;
			transform.position = pos;

			animator.SetBool("IsMoving", false);
			GetComponent<FadeAudioController>().max = false;
			return true; //Reached dest
		}

		float dir = (pos.x < destX) ? 1 : -1;
		pos.x += dir * dx;
		transform.position = pos;

		animator.SetFloat("SpeedMult", animSpeed * dir);
		animator.SetBool("IsMoving", true);
		GetComponent<FadeAudioController>().max = true;
		return false;
	}

	void FinishTask() {
		task = Task.NONE;

		//Робимо так щоб не перетерти нову ціль, якщо OnTaskFinished видасть нову таску.
		BoxController lastBox = targetBox;
		targetBox = null;

		if (OnTaskFinished != null) {
			OnTaskFinished(lastBox); //Finish turn
		}
	}

	void Log(string taskName, BoxController box) {
		Debug.Log("          " + gameObject.name + ": Task: " + taskName + " Target: " + box.gameObject.name);
	}

	//-------------------------------------------------------------------------------

	public bool IsRoverBoxHasStones() {
		if (!roverBox) {
			return false;
		}
		return roverBox.IsThereAreStones();
	}

	public void Grab(BoxController box, bool isOpponent = false) {
		Log("Grab", box);

		Debug.Assert(task == Task.NONE, gameObject.name + ": Rover is beasy!");
		Debug.Assert(box.IsThereAreStones(), gameObject.name + ": Target box is empty!");
		if ( !isOpponent ) {
			Debug.Assert(!roverBox.IsThereAreStones(), gameObject.name + ": Rover box is not empty!");
		}

		targetBox = box;
		if (isOpponent) {
			task = Task.GRAB_OPPONENT;
		} else {
			task = Task.GRAB;
			targetBox.SetMonitorOpen(false);
		}

		roverBox.AddStones(targetBox.GetAllStones());
	}

	public void Put(BoxController box, bool IsPutAll = false) {
		Log("Put", box);

		Debug.Assert(task == Task.NONE, gameObject.name + ": Rover is beasy!");
		Debug.Assert(roverBox.IsThereAreStones(), gameObject.name + ": Rover box is empty!");

		targetBox = box;

		if (IsPutAll) {
			task = Task.PUT_ALL;
			targetBox.AddStones(roverBox.GetAllStones());
		} else {
			task = Task.PUT;
			roverBox.GetOneStone();
			targetBox.AddStones(1);		
			targetBox.SetMonitorOpen(false);	
		}		
	}


	public void MoveTo(BoxController box) {
		Log("MoveTo", box);

		Debug.Assert(task == Task.NONE, gameObject.name + ": Rover is beasy!");

		targetBox = box;
		task = Task.MOVE;	
	}
	//-------------------------------------------------------------------------------

	void OnAnimFinished() {
		FinishTask();
		isAnimating = false;
	}

	void OnUpdateRoverBox() {
		roverBox.UpdateVisualBox();
	}

	void OnUpdateTargetBox() {
		targetBox.UpdateVisualBox();
		targetBox.SetMonitorOpen(true);
	}

	//-------------------------------------------------------------------------------



}
