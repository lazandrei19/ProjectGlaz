using UnityEngine;
using UnityEditor;
using System.Collections;

public class ToolsTransformScript : Editor {

	[MenuItem ("Tools/Transform/Align To Ground")]
	static void AlignToGround () {
		Transform t = Selection.activeTransform;
		Undo.RecordObject (t, "Align to Ground");
		Ray ray = new Ray (t.position, Vector3.down);
		RaycastHit hitInfo;
		Physics.Raycast (ray, out hitInfo);
		Bounds b = t.GetComponent<Renderer> ().bounds;
		t.position = hitInfo.point + new Vector3(0, b.extents.y, 0);
		t.rotation = Quaternion.LookRotation (hitInfo.transform.rotation * Vector3.forward, hitInfo.normal);
	}

	[MenuItem ("Tools/Transform/Reset Local Position")]
	static void ResetLocalPosition () {
		Undo.RecordObjects (Selection.transforms, "Reset Local Position");
		foreach (Transform t in Selection.transforms) {
			t.localPosition = Vector3.zero;
		}
	}

	[MenuItem ("Tools/Transform/Reset Global Position")]
	static void ResetGlobalPosition () {
		Undo.RecordObjects (Selection.transforms, "Reset Global Position");
		foreach (Transform t in Selection.transforms) {
			t.position = Vector3.zero;
		}
	}

	[MenuItem ("Tools/Transform/Reset Local Rotation")]
	static void ResetLocalRotation () {
		Undo.RecordObjects (Selection.transforms, "Reset Local Rotation");
		foreach (Transform t in Selection.transforms) {
			t.localEulerAngles = Vector3.zero;
		}
	}

	[MenuItem ("Tools/Transform/Reset Global Rotation")]
	static void ResetGlobalRotation () {
		Undo.RecordObjects (Selection.transforms, "Reset Global Rotation");
		foreach (Transform t in Selection.transforms) {
			t.eulerAngles = Vector3.zero;
		}
	}

	[MenuItem ("Tools/Transform/Reset Scale")]
	static void ResetScale () {
		Undo.RecordObjects (Selection.transforms, "Reset Scale");
		foreach (Transform t in Selection.transforms) {
			t.localScale = Vector3.zero;
		}
	}
}
