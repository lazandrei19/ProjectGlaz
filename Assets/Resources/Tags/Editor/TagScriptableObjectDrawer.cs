using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//[CustomEditor(typeof(TagScriptableObject))]
public class TagScriptableObjectDrawer : Editor {

	List <string> Tags;

	void OnEnable () {
		Tags = ((TagScriptableObject) target).Tags;
	}

	public override void OnInspectorGUI() {
		for (int i = 0; i < Tags.Count; i++) {
			Tags[i] = EditorGUILayout.DelayedTextField (Tags[i]);
		}
		if (GUILayout.Button ("Add New")) {
			Tags.Add ("Tag " + Tags.Count);
			Repaint ();
		}
		Tags = Tags.Distinct ().ToList ();
		((TagScriptableObject) target).Tags = Tags;
	}
}
