using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Button[] player1;
	public Button[] player2;
	public Button[] menu;

	public Button popup;

	public MMController mainMenu;

	void Start() {
		HideAll();
	}

	void PlayerMove(int index) {
		if (mainMenu.IsMonitorActive()) {
			return;
		}

		HideAll();

		PlayerController.current.MakeMove(index);
	}

	void HideAll() {
		foreach (Button button in player1) {
			button.gameObject.SetActive(false);
		}
		foreach (Button button in player2) {
			button.gameObject.SetActive(false);
		}
	}

	public void ShowPlayerButtons(Button[] buttons, int[] playerField) {
		int i = 0;
		foreach (Button button in buttons) {
			button.gameObject.SetActive(playerField[i] > 0);
			i++;
		}
		foreach (Button button in menu) {
			button.gameObject.SetActive(true);
		}
	}

	public void Popup(string text) {
		popup.transform.Find("Text").GetComponent<Text>().text = text;
		popup.gameObject.SetActive(true);
	}

	void HidePopup() {
		popup.gameObject.SetActive(false);
	}

}
