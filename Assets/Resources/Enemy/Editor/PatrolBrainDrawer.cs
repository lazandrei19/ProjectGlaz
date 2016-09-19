using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor (typeof (PatrolBrain))]
public class PatrolBrainDrawer : Editor {
	ReorderableList reordableList;
	SerializedProperty listProperty;

	void OnEnable () {
		listProperty = serializedObject.FindProperty ("e_Actions");
		reordableList = new ReorderableList (serializedObject, listProperty);
		reordableList.drawElementCallback = DrawElement;
		reordableList.drawElementBackgroundCallback = DrawElementBackground;
		reordableList.drawHeaderCallback = (Rect rect) => {
			EditorGUI.LabelField (rect, new GUIContent ("Patrol actions", "These are the actions that the enemy will take while patrolling.\nFor now, this needs to have at least one MOVE action"));
		};
		reordableList.onAddCallback = (ReorderableList list) => {
			int index = list.serializedProperty.arraySize;
			list.serializedProperty.arraySize++;
			list.index = index;
			SerializedProperty item = reordableList.serializedProperty.GetArrayElementAtIndex (index);
			item.FindPropertyRelative ("eat").enumValueIndex = 0;
			item.FindPropertyRelative ("direction").enumValueIndex = 0;
			item.FindPropertyRelative ("ammount").intValue = 1;
		};
		reordableList.onCanRemoveCallback = (ReorderableList list) => {
			return list.count > 1;
		};
	}

	public override void OnInspectorGUI () {
		serializedObject.Update();
		reordableList.DoLayoutList ();
		serializedObject.ApplyModifiedProperties();
	}

	void DrawElement (Rect rect, int index, bool isActive, bool isFocused) {
		//Centering
		rect.y += 2;
		//Initializing the item
		SerializedProperty item = reordableList.serializedProperty.GetArrayElementAtIndex (index);
		//Drawing
		EditorGUI.PropertyField(new Rect(rect.x, rect.y, 70, EditorGUIUtility.singleLineHeight), item.FindPropertyRelative("eat"), GUIContent.none);
		EditorGUI.PropertyField(new Rect(rect.x + 75, rect.y, 80, EditorGUIUtility.singleLineHeight), item.FindPropertyRelative("direction"), GUIContent.none);
		EditorGUI.IntSlider (new Rect (rect.x + 160, rect.y, rect.width - 165, EditorGUIUtility.singleLineHeight), item.FindPropertyRelative ("ammount"), 1, 10, GUIContent.none);
	}

	void DrawElementBackground (Rect rect, int index, bool isActive, bool isFocused) {
		//Centering
		rect.x += 2;
		rect.width -= 4;
		rect.y -= 1;
		//Initializing
		try {
			SerializedProperty item = reordableList.serializedProperty.GetArrayElementAtIndex (index);
			string eat = item.FindPropertyRelative ("eat").enumNames [item.FindPropertyRelative ("eat").enumValueIndex];
			string direction = item.FindPropertyRelative ("direction").enumNames [item.FindPropertyRelative ("direction").enumValueIndex];
			bool isGood = false;
			//Logic
			if (eat == "MOVE") {
				if (direction == "FORWARD" || direction == "BACKWARD" || direction == "RIGHT" || direction == "LEFT") {
					isGood = true;
				}
			} else if (eat == "ROTATE") {
				if (direction == "RIGHT" || direction == "LEFT") {
					isGood = true;
				}
			}

			if (isGood && !isActive) {
				return;
			} else if (!isGood && !isActive) {
				EditorGUI.DrawRect (rect, Color.red);
			} else if (!isGood && isActive) {
				EditorGUI.DrawRect (rect, Color.magenta);
			} else if (isGood && isActive) {
				EditorGUI.DrawRect (rect, Color.blue);
			}
		} catch (System.Exception e) {
			Debug.Log (e);
		}
	}
}