using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static GameController gc;
	public List <ScriptableObject> managers;
	Dictionary <string, object> data = new Dictionary<string, object> () {};

	void Start () {
		Tasks.Initialize ();
	}

	public void Register (string id, object obj) {
		data.Add (id, obj);
	}

	public T Get <T> (string id) {
		return ((T) (data [id]));
	}

	void Awake () {
		if (gc == null) {
			gc = this;
			DontDestroyOnLoad (this);
		} else if (gc != this) {
			Destroy (this);
		}
	}
}
