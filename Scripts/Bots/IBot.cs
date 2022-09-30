using UnityEngine;

public interface IBot {
    int OnBotMove (int[] my, int[] opponent);
}

public class Bots {
    public enum PlayerType {
		HUMAN,
		BOT_RANDOM,
		BOT
	}

    public static IBot GetBot(PlayerType playerType) {
		switch (playerType) {
			case PlayerType.HUMAN : return null;
			case PlayerType.BOT : return new Bot();
			case PlayerType.BOT_RANDOM : return new BotRandom();
		}
		return null;
	}
}