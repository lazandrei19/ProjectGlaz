using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

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

	public T Get <T> (string id, T def = default (T)) {
		if (data.ContainsKey (id)) {
			return ((T)(data [id]));
		} else {
			return def;
		}
	}

	void Awake () {
		if (gc == null) {
			gc = this;
			DontDestroyOnLoad (this);
		} else if (gc != this) {
			Destroy (this);
		}
	}

	public void Save () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = new FileStream (Application.persistentDataPath + "/save.dat", FileMode.Create);

		bf.Serialize (fs, data);
		fs.Close ();
	}
}
