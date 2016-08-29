using UnityEngine;
using System.Collections;

public enum Turn {
	PLAYER,
	ENEMY
}

public class GameloopController : MonoBehaviour {
	public static GameloopController glc;

	public Turn turn = Turn.PLAYER;

	// Use this for initialization
	void Awake () {
		if (glc == null) {
			glc = this;
			DontDestroyOnLoad (this);
		} else if (glc != this) {
			Destroy (this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
