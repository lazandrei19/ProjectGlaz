using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	GameObject enemy_prefab;

	bool hasSpawnedThisRound = false;

	// Use this for initialization
	void Start () {
		enemy_prefab = Resources.Load ("Player/Prefabs/Enemy") as GameObject;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.R)) {
			if (!hasSpawnedThisRound) {
				hasSpawnedThisRound = true;
				GameObject enemy = Instantiate (enemy_prefab);
				enemy.transform.position = new Vector3 (Random.Range (-50, 50), 2, Random.Range (-50, 50));
			}
		} else {
			hasSpawnedThisRound = false;
		}
	}
}
