using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalahArmController : MonoBehaviour {

	Transform clawCenter;
	GameObject clawOpen, clawClosed;
	Transform wheel1, wheel2;

	public Transform candyBox;
	public Queue<GameObject> targets;
	
	public Vector2 moveSpeed;
	float rotationSpeed;
	public bool invertGersRotation;

	int ind = 0;
	Vector3 dest, startPos;
	bool grabbed = false;
	GameObject target = null;

	void Awake() {
		targets = new Queue<GameObject>();

		clawCenter = transform.Find("ClawCenter");
		clawOpen = clawCenter.Find("ClawOpen").gameObject;
		clawClosed = clawCenter.Find("ClawClosed").gameObject;

		wheel1 = transform.Find("Wheel1");
		wheel2 = transform.Find("Wheel2");

		rotationSpeed = moveSpeed.x * 20;

		if (invertGersRotation) {
			rotationSpeed *= -1;
		}

		startPos = clawCenter.position;
	}

	// Use this for initialization
	void Start () {
		SetClawOpen(!grabbed);
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null && targets.Count > 0) {
			target = targets.Dequeue();
			dest = GetCandyBoxSlotPosByIndex(ind);
			ind++;
		} 

		if (target != null) {
			if (!grabbed) {
				if ( IsReachedTo(target.transform.position) ) {
					target.transform.parent = clawCenter;
					target.transform.localPosition = Vector3.zero;
					target.transform.localRotation = Quaternion.identity;
					
					grabbed = true;
					SetClawOpen(!grabbed);
				}
			} else {
				if ( IsReachedTo(dest) ) {
					target.transform.parent = candyBox;					
					grabbed = false;
					SetClawOpen(!grabbed);
					target = null;
				}
			}
		} else {
			IsReachedTo(startPos);
		}
	}

	bool IsReachedTo(Vector3 targetPos) {
		Vector3 curPos = transform.position;

		curPos.x = Mathf.MoveTowards(curPos.x, targetPos.x, moveSpeed.x * Time.deltaTime);
		transform.position = curPos;

		float distX = targetPos.x - curPos.x;
		bool isReachedX = Mathf.Abs(distX) < 0.003f;
		if (!isReachedX) {
			if (distX > 0) {
				wheel1.Rotate(new Vector3(0, 0, -rotationSpeed));
				wheel2.Rotate(new Vector3(0, 0, -rotationSpeed));
			} else {
				wheel1.Rotate(new Vector3(0, 0, rotationSpeed));
				wheel2.Rotate(new Vector3(0, 0, rotationSpeed));
			}
		}

		curPos = clawCenter.position;
		curPos.y = Mathf.MoveTowards(curPos.y, targetPos.y, moveSpeed.y * Time.deltaTime);
		clawCenter.position = curPos;

		bool isReachedY = Mathf.Abs(targetPos.y - curPos.y) < 0.003f;

		return isReachedX && isReachedY;
	}

	void SetClawOpen(bool value) {
		clawOpen.SetActive(value);
		clawClosed.SetActive(!value);
	}

	Vector3 GetCandyBoxSlotPosByIndex(int index) {
		const int cellsInRow = 5;
		int row = index / cellsInRow - 2;
		int col = index % cellsInRow - 2;

		float dx = 0.05f;
		float dy = 0.08f; 

		Vector3 pos = candyBox.position;
		pos.x += col * dx;
		pos.y -= row * dy;

		return pos;
	}

	//-----------------------------------------------------------

	public void AddTarget(GameObject target) {
		targets.Enqueue(target);
	}
}
