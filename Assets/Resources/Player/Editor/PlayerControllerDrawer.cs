using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerDrawer : Editor {

	//All fields from PlayerController
	SerializedProperty CameraReference;
	SerializedProperty EyesReference;

	void OnEnable () {
		//Initialization of all property fields

		CameraReference = serializedObject.FindProperty ("er_CameraTransform");
		EyesReference = serializedObject.FindProperty ("er_EyesTransform");
	}

	public override void OnInspectorGUI() {
		//Update's the serialized object representation to be able to undo
		serializedObject.Update();

		//Field
		EditorGUILayout.PropertyField (CameraReference, new GUIContent ("Camera Reference", "A reference to the Camera that follows the player"));
		EditorGUILayout.PropertyField (EyesReference, new GUIContent ("Eyes Reference", "A reference to the eyes of the player. Used for raycasting and determining if movement is possible"));

		//Save modified changes
		serializedObject.ApplyModifiedProperties();
	}
}
