using UnityEngine;
using System.Collections;

public class QuickResetScript : MonoBehaviour {

	void Update () {
		if (transform.position.y < -5)
			transform.position = new Vector3 (0, 10, 0);
	}
}
