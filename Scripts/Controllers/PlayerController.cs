using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public delegate void Finished(PlayerController player);
	public event Finished OnMoveFinished;
	public event Finished OnInitFinished;


	public RoverController rover;
	public BoxController kalah;
	public BoxController[] boxes;
	public PlayerController opponent;

	public Transform rollersParent;
	RollerController[] rollers;

	static public PlayerController current;

	enum State {
		IDLE,
		INIT, 
		MAKING_MOVE,
		GRABING_OPPONENT_BOX,
		START_CAPTURE,
		CONTINUE_CAPTURE,
		FINISH_CAPRUTE,
		CAPTURED
	}

	State state;

	void Awake() {
		rover.OnTaskFinished += OnRoverTaskFinished;
		state = State.IDLE;

		rollers = rollersParent.GetComponentsInChildren<RollerController>();
	}

	void Start() {
		for (int i = 0; i < rollers.Length; i++) {
			rollers[i].PlayAnim(true, 1.0f + i * 0.05f);
		}
	}

	void StopRollers() {
		for (int i = 0; i < rollers.Length; i++) {
			rollers[i].PlayAnim(false, 0);
		}
	}

	void OpenMonitor(int index){
		boxes[index].SetMonitorOpen(true);
	}
//------------------------------------------------------------------------------------------

	void OnRoverTaskFinished(BoxController lastBox) {
		Debug.Log(gameObject.name + ": rover finished task " + state.ToString());

		switch (state) {
			case State.INIT: 
				state = State.IDLE;
				OnInitFinished(this);
			break;

			//STEP 2
			case State.CAPTURED: //Захоплюють. Взяв свої
				opponent.ContinueCapture(rover.RoverBox); //Яказати опоненту що нехай забирає свої кляті камінці і вдавиться ПАСКУДА! :D
				state = State.IDLE;
			break;

			//STEP 1
			case State.START_CAPTURE: //Захоплюю. Вже взяв свій
				int oppositeIndex = GetOppositeIndex(lastBox);
				opponent.GrabForCapruting(oppositeIndex); //Змусити опонента взяти його камінці в ровер
			break;

			//STEP 3
			case State.CONTINUE_CAPTURE: //Захоплюю. Взяв з ровера опонента
				FinishCapture();
			break;

			//STEP 4
			case State.FINISH_CAPRUTE: //Захопив
				FinitshMove(true);
			break;


			case State.GRABING_OPPONENT_BOX:
			case State.MAKING_MOVE:
				if ( rover.IsRoverBoxHasStones() ) { //Чи є ще камінці в моєму ровері?
					BoxController next = lastBox.nextBox;

					//Якщо є наступна коробка
					//Якщо наступна коробка не калах
					//Якщо наступна коробка калах але це мій хід
					if (next && (!next.isKalah || (next.isKalah && IsMyMove())) ) {
						rover.Put(next);
					} else {
						//Передати решту камінців роботу опонента
						opponent.GrabBoxFromOpponentRover(rover.RoverBox);
						state = State.IDLE;
					}
				} else { //Якщо ровер виклав всі камінці...

					if ( !IsMyMove() ) { //Якщо закінчили на стороні опонента...
						FinitshMove(true);
					} else {
						if ( lastBox.isKalah ) {
							FinitshMove(false);
						} else {
							//Якщо останній камінець поклали в пусту коробку
							if (lastBox.Stones == 1) { 
								TryToCapture(lastBox);
							} else {
								FinitshMove(true);
							}
						}
					}
				}
			break;
		}
		
	}

	void FinitshMove(bool isPlayerSwitching) {
		state = State.IDLE;
		if (isPlayerSwitching) {
			current = current.opponent;
		}
		OnMoveFinished(this);
	}

	void Log(string msg) {
		Debug.Log(gameObject.name + ": " + msg);
	}

	//------------------------------------------------------------------------------

	bool IsMyMove() {
		return current == this;
	}

	void GrabBoxFromOpponentRover(BoxController opponentBox) {
		Log("GrabBoxFromOpponentRover " + opponentBox.gameObject.name);
		Debug.Assert(opponentBox.IsThereAreStones(), "Opponent box is empty!");

		state = State.GRABING_OPPONENT_BOX;
		opponentBox.nextBox = boxes[0]; //Коли заберемо коробку у опонента, то наступною буде моя перша коробка
		rover.Grab(opponentBox, true);
	}

	void GrabForCapruting(int index) {
		Log("GrabForCapruting " + index);
		Debug.Assert(index >= 0 && index < boxes.Length, "Wrong box index");

		state = State.CAPTURED;
		rover.Grab(boxes[index]);
	}

	void TryToCapture(BoxController myLastBox) {
		Log("ContinueCapture " + myLastBox.gameObject.name);
		int oppositeIndex = GetOppositeIndex(myLastBox);
 		if (opponent.StonesInBox(oppositeIndex) <= 0) { //якщо немає чого захвачувати
			FinitshMove(true);
			return;
		}

		state = State.START_CAPTURE;
		rover.Grab(myLastBox); //Забираю той один який у мене
	}

	void ContinueCapture(BoxController opponentBox) {
		Log("ContinueCapture " + opponentBox.gameObject.name);
		state = State.CONTINUE_CAPTURE;
		rover.Grab(opponentBox, true); //забираю захвачені з ровера опонента
	}

	void FinishCapture() {
		Log("FinishCapture");
		BoxController box = boxes[boxes.Length - 1].nextBox;
		Debug.Assert(box && box.isKalah, "IS NOT KALAH!!!");

		state = State.FINISH_CAPRUTE;
		rover.Put(box, true);
	}

	int GetOppositeIndex(BoxController box) {
		for (int i = 0; i < boxes.Length; i++) {
			if (box == boxes[i]) {
				return boxes.Length - i - 1;
			}
		}
		return -1;
	}

	//------------------------------------------------------------------------------

	public int StonesInBox(int index) {
		if ( index >= 0 && index < boxes.Length ) {
			return boxes[index].Stones;
		}
		return 0;
	}

	public int StonesInKalah() {
		return kalah.Stones;
	}

	public void StartInit() {
		rover.MoveTo(boxes[0]);
		state = State.INIT;
	}

	public void MakeMove(int index) {
		Log("MakeMove: " + index);
		Debug.Assert(IsMyMove(), gameObject.name + ": It is NOT my move");
		Debug.Assert(index >= 0 && index < boxes.Length, gameObject.name + ": Wrong box index");
		
		state = State.MAKING_MOVE;
		rover.Grab(boxes[index]);
	}

	public void InitPlayer(int[] data) {
		for (int i = 0; i < boxes.Length; i++) {
			boxes[i].GetAllStones();
			boxes[i].AddStones(data[i]);
			boxes[i].UpdateVisualBox();
		}
		kalah.GetAllStones();
		kalah.UpdateVisualBox();
	}

	public void InitPlayer(int data) {
		for (int i = 0; i < boxes.Length; i++) {
			boxes[i].GetAllStones();
			boxes[i].AddStones(data);
			boxes[i].UpdateVisualBox();
		}
		kalah.GetAllStones();
		kalah.UpdateVisualBox();
	}
}
