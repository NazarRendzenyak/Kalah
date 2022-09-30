using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ActionPhaseMenu : MonoBehaviour {

	public void BackToMainMenu()
	{
		SceneManager.LoadScene("SocialMap");
	}

	public void ShowConfirm()
	{
		GameObject CpGo =  GameObject.Find("ConfirmationPopup");
		if(CpGo != null)
		{
			var cp = CpGo.GetComponent<Canvas>();
			cp.enabled = true;
		}
	}

	public void HideConfirm()
	{
		GameObject CpGo =  GameObject.Find("ConfirmationPopup");
		if(CpGo != null)
		{
			var cp = CpGo.GetComponent<Canvas>();
			cp.enabled = false;
		}
	}
}
