using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomEditor(typeof(Tags))]
public class TagDrawer : Editor {

	List<string> Tags = new List<string> ();
	Dictionary<string, bool> EnabledTags = new Dictionary<string, bool> ();
	TagScriptableObject AllTags;
	Vector2 ScrollPos;
	string SearchQuery = "";
	List<string> Keys = new List<string> ();

	void OnEnable () {
		Tags = ((Tags) target).e_Tags;
		AllTags = (TagScriptableObject) Resources.Load ("Tags/ScriptableObjects/Tags");
		foreach (string tag in AllTags.Tags) {
			EnabledTags.Add (tag, Tags.Contains (tag));
		}
		Keys = EnabledTags.Keys.ToList ();
	}

	public override void OnInspectorGUI() {
		serializedObject.Update ();
		SearchQuery = EditorGUILayout.TextField (SearchQuery);
		Keys = EnabledTags.Keys.Where ((string tag) => tag.Contains (SearchQuery)).ToList ();
		ScrollPos = EditorGUILayout.BeginScrollView (ScrollPos, GUILayout.Height (250));
		for (int i = 0; i < Keys.Count; i++) {
			string key = Keys [i];
			EnabledTags[key] = EditorGUILayout.ToggleLeft (key, EnabledTags[key]);
			if (EnabledTags[key] == true) {
				if (!Tags.Contains<string> (key)) {
					Tags.Add (key); 
				}
			} else {
				if (Tags.Contains<string> (key)) {
					Tags.Remove (key); 
				}
			}
			Tags = Tags.Distinct ().ToList ();
		}
		EditorGUILayout.EndScrollView ();
		((Tags) target).e_Tags = Tags;
		if (GUILayout.Button ("Dump Tags")) {
			string tags = "";
			for (int i = 0; i < ((Tags) target).e_Tags.Count - 1; i++) {
				tags += ((Tags) target).e_Tags[i] + ", ";
			}
			tags += ((Tags) target).e_Tags [((Tags) target).e_Tags.Count - 1];
			Debug.Log (tags);
		}
	}
}

