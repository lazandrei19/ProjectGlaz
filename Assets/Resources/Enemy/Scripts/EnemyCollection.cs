using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public struct Enemy {
	public EnemyController enemy;
	public bool done;

	public Enemy (EnemyController e) {
		enemy = e;
		done = false;
	}
}

public class EnemyCollection {

	List <Enemy> enemies;

	public bool isDone {
		get {
			return enemies.Count (e => e.done) == enemies.Count;
		}
	}

	bool calledAll = false;

	public void Reset () {
		calledAll = false;
		enemies.ForEach (e => e.enemy.PrepareForNextThink ());
		enemies.ForEach (delegate(Enemy e) {
			int index = enemies.IndexOf (e);
			e.done = false;
			enemies [index] = e;
		});
	}

	public void Add (EnemyController enemy) {
		enemies.Add (new Enemy (enemy));
	}

	public void Done (EnemyController enemy) {
		Enemy foundEnemy = enemies.Find (e => e.enemy == enemy);
		int index = enemies.IndexOf (foundEnemy);
		foundEnemy.done = true;
		enemies [index] = foundEnemy;
	}

	public void Think () {
		if (calledAll) {
			return;
		}

		enemies.ForEach (enemy => enemy.enemy.Think ());
		calledAll = true;
	}

	public EnemyCollection () {
		enemies = new List<Enemy> ();
	}
}
