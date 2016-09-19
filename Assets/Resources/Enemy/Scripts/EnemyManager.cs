using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	static EnemyCollection enemies = new EnemyCollection ();

	public static void Register (EnemyController enemy) {
		enemies.Add (enemy);
	}

	public static void Done (EnemyController enemy) {
		enemies.Done (enemy);
	}
	
	// Update is called once per frame
	void Update () {
		if (enemies.isDone) {
			GameController.gc.Get <GameloopController> ("Managers/GLC").turn = Turn.PLAYER;
			enemies.Reset ();
		}

		if (GameController.gc.Get <GameloopController> ("Managers/GLC").turn != Turn.ENEMY) {
			return;
		}

		enemies.Think ();
	}
}
