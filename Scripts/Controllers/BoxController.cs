using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

	public MonitorController monitor;
	public BoxController nextBox;
	public bool isKalah;

	private int stones = 0;
	private CandiesController candies;

	void Awake() {
		candies = GetComponent<CandiesController>();
	}

	public int Stones {
		get { return stones; }
	}

	void Start() {
		UpdateVisualBox();
	}

	//-------------------------------------------------------------------------

	public void AddStones(int value) {
		stones += value;
	}

	public bool GetOneStone() {
		if (stones > 0) {
			stones--;
			return true;
		}
		return false;
	}

	public int GetAllStones() { //capture
		int value = stones;
		stones = 0;
		return value;
	}

	public bool IsThereAreStones() {
		return stones > 0;
	}

	public void UpdateVisualBox() {
		if (monitor) {
			monitor.SetNumber(stones);
		}
		if (candies != null) {
			candies.SetNumber(stones);
		}			
	}

	public void SetMonitorOpen(bool isOpen) {
		monitor.SetOpen(isOpen);
	}
}
