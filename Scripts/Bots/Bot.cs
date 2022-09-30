using UnityEngine;

public class Bot : IBot
{

	public int OnBotMove (int[] my, int[] opponent){
        Debug.Log("Thinking...");
        int cellIndex = -1;
        
        if(FindWinningIndex(my) != -1)
        {
            Debug.Log("Winning index");
            cellIndex = FindWinningIndex(my);
        }
        else if(FindCatchingIndex(my, opponent) != -1)
        {
            Debug.Log("Catchedindex");
            cellIndex = FindCatchingIndex(my, opponent);
        }
        else
        {
            Debug.Log("Nearest index");
            cellIndex = FindNearesBiggerCapacityIndex(my);
        }

        Debug.Log("BotRandom: my move is " + cellIndex);

		return cellIndex;
    }

    public int FindWinningIndex(int[] my)
	{
		for(int index = my.Length - 1; index >= 0; index--)
		{
			int lengthToKalah = my.Length - index;
			if(my[index] == lengthToKalah)
			{
				return index;
			}
		}
		return -1;
	}

	public int FindCatchingIndex(int[] my, int[] opponent)
	{
		int index = -1;
		for(int i = my.Length -1; i >= 0; i--)
		{
			int zeroCellIndex = FindNextIndexWithZeroCapacity(i, my);
			if(zeroCellIndex != -1)
			{
				index = FindIndexToCatchZeroCell(zeroCellIndex, my, opponent);
				if(index == -1)
				{
					i = zeroCellIndex;
				}
			}
		}
		return index;
	}

	public int FindNearesBiggerCapacityIndex(int[] my)
	{
		int biggerCapacity = 0;
		int index = -1;

		for(int i = my.Length - 1; i >= 0; i--)
		{
			if(my[i] >= biggerCapacity)
			{
				biggerCapacity = my[i];
				index = i;
			}
		}
		return index;
	}

	private int FindNextIndexWithZeroCapacity(int startIndex, int[] my)
	{
		for(int i = startIndex; i >= 0; i--)
		{
			if(my[i] == 0)
			{
				return i;
			}
		}
		return -1;
	}

	private int FindIndexToCatchZeroCell(int zeroCellIndex, int[] my, int[] opponent)
	{
		for(int i = my.Length -1; i >= 0; i--)
		{
			int lengthToZeroCapacityCell = zeroCellIndex - i;
			if(my[i] !=0 && my[i] == lengthToZeroCapacityCell && opponent[i] != 0)
			{
				return i;
			}
		}
        return -1;
	}
}