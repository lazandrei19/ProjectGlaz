using UnityEngine;
using System.Collections;

public enum Turn {
	PLAYER,
	ENEMY
}

public class GameloopController {
	public Turn turn;

	public GameloopController () {
		turn = Turn.PLAYER;
	}
}
