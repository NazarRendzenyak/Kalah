using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxesAnimEvents : MonoBehaviour {

	public PlayerAnimController playerAnimController;

	public void StopRollers(){
		playerAnimController.EnableRollers(false);		
	}
}
