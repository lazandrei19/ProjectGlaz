using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tags : MonoBehaviour {
	public List<string> e_Tags;

	void Reset () {
		e_Tags = new List<string> () { };
	}

	public bool HasTag (string tag) {
		return e_Tags.Contains (tag);
	}
}

