using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerController : MonoBehaviour {

	private Animator animator;

	void Awake() {
		//float animOffset = Random.value;

		animator = GetComponent<Animator>();
		//animator.SetFloat("Offset", animOffset);
		animator.enabled = false;
	}

	public void PlayAnim(bool value, float delay = 0) {
		if (animator.enabled != value) {
			Invoke("ChangeAnimatrorState", delay);
		}
	}

	void ChangeAnimatrorState() {
		animator.enabled = !animator.enabled;
	}
}
