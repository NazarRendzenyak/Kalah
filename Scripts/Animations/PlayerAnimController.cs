using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour {

	public float speedMult = 1.0f;
	public Animator boxesAmin;
	public Animator[] rollerAmins;

	void Start () {
		boxesAmin.SetFloat("SpeedMult", speedMult);
		foreach (Animator rollerAmin in rollerAmins) {
			float offset = Random.value;
			rollerAmin.SetFloat("SpeedMult", speedMult);
			rollerAmin.SetFloat("Offset", offset);
			//rollerAmin.enabled = false;
		}
	}
	
	public void EnableRollers(bool value) {
		foreach (Animator rollerAmin in rollerAmins) {
			rollerAmin.enabled = value;
		}
	}
}
