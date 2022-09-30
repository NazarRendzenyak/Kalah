using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandiesController : MonoBehaviour {

	public Sprite[] sprites;
	public GameObject prefab;

	List<GameObject> candies;

	public KalahArmController kalahArm;
	public Transform spawnPosition;

	void Awake() {
		candies = new List<GameObject>();
		if (!spawnPosition) {
			spawnPosition = gameObject.transform;
		}
	}

	public void SetNumber(int number) {
		for (int i = 0; i < candies.Count; i++) {
			candies[i].SetActive(i < number);
		}

		while (candies.Count < number) {
			GameObject candy = CreateCandy();
			candies.Add(candy);
			if (kalahArm) {
				kalahArm.AddTarget(candy);
			}
		}
	}

	GameObject CreateCandy() {
		GameObject candy = Instantiate(prefab, spawnPosition, false);
		SpriteRenderer rend = candy.GetComponent<SpriteRenderer>();

		rend.sprite = sprites[Random.Range(0, sprites.Length)];
		rend.sortingOrder = 2;
		rend.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

		float x = Random.Range(-0.08f, 0.08f);
		float y = Random.Range(-0.08f, 0.08f);
		candy.transform.localPosition = new Vector3(x, y, 0);
		candy.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 30));

		return candy;
	}
}
