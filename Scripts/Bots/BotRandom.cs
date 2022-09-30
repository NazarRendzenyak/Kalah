using UnityEngine;

public class BotRandom : IBot
{
    public int OnBotMove (int[] my, int[] opponent) {
        int cellIndex = 0;

        do {
            cellIndex = Random.Range(0, my.Length);
        } while(my[cellIndex] == 0);

        return cellIndex;
    }
}