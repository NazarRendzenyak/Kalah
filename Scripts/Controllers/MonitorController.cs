using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorController : MonoBehaviour {

	public Animator center, left, right;
	public bool invertNumbers;

	Animator animator;
	int number;
	
	void Awake() {
		animator = GetComponent<Animator>();
	}
	
	void Start () {
		center.speed = 0;
		left.speed = 0;
		right.speed = 0;

		if (invertNumbers) {
			Vector3 scale = new Vector3(-1, -1, 1);
			center.gameObject.transform.localScale = scale;
			left.gameObject.transform.localScale = scale;
			right.gameObject.transform.localScale = scale;
		}
	}
	
	public void SetNumber(int number) {
		this.number = Mathf.Min(Mathf.Max(number, 0), 99);

		bool oneDigit = number < 10;

		center.gameObject.GetComponent<SpriteRenderer>().enabled = oneDigit;
		left.gameObject.GetComponent<SpriteRenderer>().enabled = !oneDigit;
		right.gameObject.GetComponent<SpriteRenderer>().enabled = !oneDigit;

		if ( oneDigit ) {
			float frame = 0.1f * number;
			if (center.gameObject.activeSelf) {
				center.Play("Numbers", 0, frame);
			}
		} else {
			int leftDigit = number / 10;
			int rightDigit = number % 10;
			float frameLeft = 0.1f * leftDigit;
			float frameRight = 0.1f * rightDigit;
			if ( !invertNumbers ) {
				left.Play("Numbers", 0, frameLeft);
				right.Play("Numbers", 0, frameRight);			
			} else {
				left.Play("Numbers", 0, frameRight);
				right.Play("Numbers", 0, frameLeft);
			}
		}
	}

	public void SetOpen(bool isOpen) {
		animator.SetBool("Open", isOpen);	
	}

	public void OnChangeState(int state) {
		center.gameObject.SetActive(state == 1);
		left.gameObject.SetActive(state == 1);
		right.gameObject.SetActive(state == 1);
		SetNumber(number);
	}
}
