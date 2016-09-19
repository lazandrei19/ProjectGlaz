using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameloopController {
	public Turn turn;

	public GameloopController () {
		turn = Turn.PLAYER;
	}
}
