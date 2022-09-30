using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public delegate void GameFinished(int playerIndex, int scoreRight, int scoreLeft); //0-draw 1-first 2-second
	public event GameFinished OnGameFinished;

	IBot bot1;
	IBot bot2;

	public Bots.PlayerType typeOfPlayer1;
	public Bots.PlayerType typeOfPlayer2;
	
	PlayerController player1;
	PlayerController player2;

	UIController ui;

	public const int INITIAL_STONES = 4;
	public const int FIELD_SIZE = 6;
	public const int WIN_STONES = INITIAL_STONES * FIELD_SIZE;
	
	int[] player1field = new int[FIELD_SIZE];
	int[] player2field = new int[FIELD_SIZE];

	int player1sum = 0;
	int player2sum = 0;

	void Awake() {
		player1 = GameObject.Find("Player1").GetComponent<PlayerController>();
		player2 = GameObject.Find("Player2").GetComponent<PlayerController>();

		ui = GameObject.Find("CanvasUI").GetComponent<UIController>();

		player1.OnMoveFinished += OnPlayerMoveFinish;
		player2.OnMoveFinished += OnPlayerMoveFinish;

		player1.OnInitFinished += OnPlayerInitFinish;
		player2.OnInitFinished += OnPlayerInitFinish;

		PlayerController.current = player1;

		bot1 = Bots.GetBot(typeOfPlayer1);
		bot2 = Bots.GetBot(typeOfPlayer2);
	}

	// Use this for initialization
	void Start () {
		player1.InitPlayer(INITIAL_STONES);
		player2.InitPlayer(INITIAL_STONES);


		//Test data
		//int[] p1 = {1, 1, 2, 0, 0, 0};
		//int[] p2 = {1, 1, 1, 2, 0, 0};
		//player1.InitPlayer(p1);
		//player2.InitPlayer(p2);
	}

	public void StartGame() { //restart???
		bot1 = Bots.GetBot(typeOfPlayer1);
		bot2 = Bots.GetBot(typeOfPlayer2);
		
		player1.InitPlayer(INITIAL_STONES);
		player2.InitPlayer(INITIAL_STONES);

		player1.StartInit();
		player2.StartInit();
	}

	void OnPlayerInitFinish(PlayerController player) {
		if ( player == player1 ) { 
			OnPlayerMoveFinish(player);
		}
	}

	void OnPlayerMoveFinish(PlayerController player) {
		UpdateGameData();

		if (IsEndGame()) {
			return;
		}

		if ( PlayerController.current == player1 ) { 
			if (bot1 != null) {
				PlayerController.current.MakeMove(bot1.OnBotMove(player1field, player2field)); 
			} else {
				ui.ShowPlayerButtons(ui.player1, player1field);
			}
		} else {
			if (bot2 != null){
				PlayerController.current.MakeMove(bot2.OnBotMove(player2field, player1field)); 
			} else {
				ui.ShowPlayerButtons(ui.player2, player2field);
			}
		}
	}
	//------------------------------------------------------------

	bool IsEndGame() {
		int player1stones = player1.StonesInKalah() + player1sum;
		int player2stones = player2.StonesInKalah() + player2sum;

		if (player1sum == 0 || player2sum == 0) {
			if (player1stones > player2stones) {
				OnGameFinished?.Invoke(1, player1stones, player2stones); // player 1 wins
				return true;
			} else if (player1stones < player2stones) {
				OnGameFinished?.Invoke(2, player1stones, player2stones); // player 2 wins
				return true;
			} else {
				OnGameFinished?.Invoke(0, player1stones, player2stones); //draw
				return true;
			}
		} else if (player1.StonesInKalah() > WIN_STONES ) {
			OnGameFinished?.Invoke(1, player1stones, player2stones); // player 1 wins
			return true;
		} else if (player2.StonesInKalah() > WIN_STONES ) {
			OnGameFinished?.Invoke(2, player1stones, player2stones); // player 2 wins
			return true;
		}

		return false;
	}

	void UpdateGameData() {
		player1sum = 0;
		player2sum = 0;
		for (int i = 0; i < FIELD_SIZE; i++) {
			player1field[i] = player1.StonesInBox(i);
			player2field[i] = player2.StonesInBox(i);
			player1sum += player1field[i];
			player2sum += player2field[i];
		}

		////////DEBUG
		string debug1 = "";
		string debug2 = "";
		for (int i = 0; i < FIELD_SIZE; i++) {
			debug1 += player1.StonesInBox(i)   + " ";
			debug2 += player2.StonesInBox(5-i) + " ";
		}

		Debug.Log("[ " + debug2 + "] s = " + player2sum);
		Debug.Log("[ " + debug1 + "] s = " + player1sum);
		////////////////
	}
}
