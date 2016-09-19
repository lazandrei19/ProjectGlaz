using UnityEngine;
using System.Collections;

public abstract class IEnemyBrain : ScriptableObject {
	public abstract InputFeeder Think (EnemyController enemyController, InputFeeder inputFeeder);
}
