using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMController : MonoBehaviour {

	public enum FaceType {
		EASY,
		HARD,
		PVP,
		BOTS,
		CONTINUE,
		EXIT,
	}

	[System.Serializable]
	public class Face {
		public FaceType type;
		public Image face;
	}

	public Face[] facesMain;
	public Face[] facesPause;
	int faceIndex = 0;
	Face[] faces;

	Game gameController;

	public BlurController blur;
	public UIMonitorAnimController uiMonitor;
	public GameObject buttons;

	public float initBlurDelay = 3.0f;

	bool isInit = true;
	bool isActive = true;


	public Image WinIcon;
	public Image LoseIcon;
	public Image DrawIcon;
	bool isWin = false;
	bool isLose = false;
	bool isDraw = false;
	public WinLoseScoreController score;

	public AudioClip switchClip;
	public AudioClip winClip;
	public AudioClip loseClip;
	public AudioClip drawClip;

	AudioSource audioSrc;
	public FadeAudioController musicAudioController;

	public void Awake() {
		audioSrc = GetComponent<AudioSource>();

		gameController = GameObject.Find("Game").GetComponent<Game>();
		gameController.OnGameFinished += OnGameFinished;

		buttons.SetActive(false);

		uiMonitor.OnShowFinished += ShowButtons;
		faces = facesMain;
		faceIndex = 0;
	}

	public bool IsMonitorActive() {
		return isActive || uiMonitor.IsVisible || uiMonitor.IsAnimating;
	}

	public void Update() {
		if (isInit) {
			initBlurDelay -= Time.deltaTime;
			if (initBlurDelay > 0.0f) {
				return;
			}
			isInit = false;
			blur.SetBluredState(true);
			uiMonitor.ShowUIMonitor();
		} else {
			if (Input.GetKeyUp(KeyCode.Escape)) {
				ShowPauseMenu();
			}
		}
	}

	//UI buttons

	void ShowButtons() {
		buttons.SetActive(true);
		UpdateFace();
	}

	public void PlayGame() {
		if (isWin || isLose || isDraw) {
			return;
		}

		if (faces[faceIndex].type == FaceType.EXIT) {
			audioSrc.PlayOneShot(switchClip);
			Application.Quit();
			return;
		}

		isActive = false;
		blur.SetBluredState(false);
		buttons.SetActive(false);
		uiMonitor.HideUIMonitor();
		if (faces[faceIndex].type != FaceType.CONTINUE) {
			isWin = false;
			isLose = false;
			isDraw = false;
			gameController.StartGame();
		}
		audioSrc.PlayOneShot(switchClip);
	}

	public void Left() {
		isWin = false;
		isLose = false;
		isDraw = false;
		audioSrc.PlayOneShot(switchClip);
		faceIndex--;
		UpdateFace();
	}

	public void Right() {
		isWin = false;
		isLose = false;
		isDraw = false;
		audioSrc.PlayOneShot(switchClip);
		faceIndex++;
		UpdateFace();
	}

	void UpdateFace() {
		if (faceIndex < 0) {
			faceIndex = faces.Length - 1;
		}
		if (faceIndex >= faces.Length) {
			faceIndex = 0;
		}

		foreach (Face f in facesMain) {
			f.face.gameObject.SetActive(false);
		}
		foreach (Face f in facesPause) {
			f.face.gameObject.SetActive(false);
		}

		//>>>>>>>>>>>>>>>>> ugly dirty hack
		WinIcon.gameObject.SetActive(false);
		LoseIcon.gameObject.SetActive(false);
		DrawIcon.gameObject.SetActive(false);
		score.SetVisibility(false);
		if (isWin) {
			WinIcon.gameObject.SetActive(true);
			audioSrc.PlayOneShot(winClip);
			score.SetVisibility(true);
		} else if (isLose) {
			LoseIcon.gameObject.SetActive(true);
			audioSrc.PlayOneShot(loseClip);
			score.SetVisibility(true);
		} else if (isDraw) {
			DrawIcon.gameObject.SetActive(true);
			audioSrc.PlayOneShot(drawClip);
		} else {
			faces[faceIndex].face.gameObject.SetActive(true); //this is OK
		}
		//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

		if (faces[faceIndex].type == FaceType.EASY) {
			gameController.typeOfPlayer1 = Bots.PlayerType.HUMAN;
			gameController.typeOfPlayer2 = Bots.PlayerType.BOT_RANDOM;
		}
		
		if (faces[faceIndex].type == FaceType.HARD) {
			gameController.typeOfPlayer1 = Bots.PlayerType.HUMAN;
			gameController.typeOfPlayer2 = Bots.PlayerType.BOT;
		}

		if (faces[faceIndex].type == FaceType.PVP) {
			gameController.typeOfPlayer1 = Bots.PlayerType.HUMAN;
			gameController.typeOfPlayer2 = Bots.PlayerType.HUMAN;
		}
		
		if (faces[faceIndex].type == FaceType.BOTS) {
			gameController.typeOfPlayer1 = Bots.PlayerType.BOT_RANDOM;
			gameController.typeOfPlayer2 = Bots.PlayerType.BOT;
		}
	}

	public void ShowPauseMenu() {
		if (!IsMonitorActive()) {
			blur.SetBluredState(true);
			uiMonitor.ShowUIMonitor();
			isActive = true;
			faces = facesPause;
			faceIndex = 0;
		}
	}

	void OnGameFinished(int playerIndex, int scoreRight, int scoreLeft) {
		isDraw = playerIndex == 0;
		isWin = playerIndex == 1;
		isLose = playerIndex == 2;

		if (isWin) {
			musicAudioController.SetFadeTimer(winClip.length + 1f);
		}
		if (isLose) {
			musicAudioController.SetFadeTimer(loseClip.length + 1f);
		}
		if (isDraw) {
			musicAudioController.SetFadeTimer(drawClip.length + 1f);
		}

		score.SetScrore(scoreRight, scoreLeft);

		ShowPauseMenu();
		faces = facesMain;
	}
}
