using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseScoreController : MonoBehaviour
{
    public Sprite[] numbers;

    public Image p1_1;
    public Image p1_2;

    public Image p2_1;
    public Image p2_2;

    public void SetScrore(int p1, int p2) {
        p1_1.sprite = numbers[p1 / 10];
        p1_2.sprite = numbers[p1 % 10];

        p2_1.sprite = numbers[p2 / 10];
        p2_2.sprite = numbers[p2 % 10];
    }

    public void SetVisibility(bool isVisible) {
        p1_1.gameObject.SetActive(isVisible);
        p1_2.gameObject.SetActive(isVisible);
        p2_1.gameObject.SetActive(isVisible);
        p2_2.gameObject.SetActive(isVisible);
    }
}
