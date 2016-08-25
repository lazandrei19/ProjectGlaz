using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerDrawer : Editor {

	//All fields from PlayerController
	SerializedProperty CameraReference;
	SerializedProperty EyesReference;
	SerializedProperty RotateAmount;
	SerializedProperty CameraMovementRange;

	//Helpers
	float minVal, maxVal;

	void OnEnable () {
		//Initialization of all property fields
		CameraReference = serializedObject.FindProperty ("er_CameraTransform");
		EyesReference = serializedObject.FindProperty ("er_EyesTransform");
		RotateAmount = serializedObject.FindProperty ("e_RotateAmount");
		CameraMovementRange = serializedObject.FindProperty ("e_CameraMovementRange");
		minVal = CameraMovementRange.vector2Value.x;
		maxVal = CameraMovementRange.vector2Value.y;
	}

	public override void OnInspectorGUI() {
		//Update's the serialized object representation to be able to undo
		serializedObject.Update();

		//Field
		EditorGUILayout.PropertyField (CameraReference, new GUIContent ("Camera Reference", "A reference to the Camera that follows the player"));
		EditorGUILayout.PropertyField (EyesReference, new GUIContent ("Eyes Reference", "A reference to the eyes of the player. Used for raycasting and determining if movement is possible"));

		EditorGUILayout.Space ();
		RotateAmount.floatValue = EditorGUILayout.IntSlider (new GUIContent ("Rotate amount", "The amount of degrees to rotate when pressing the rotate button once"), Mathf.RoundToInt (RotateAmount.floatValue), 1, 360);

		EditorGUILayout.Space ();
		EditorGUILayout.LabelField (new GUIContent ("Camera movement range (" + minVal + ", " + maxVal + ")", "The values between the camera moves when trying to avoid clipping"));
		EditorGUILayout.MinMaxSlider (new GUIContent (""), ref minVal, ref maxVal, -15f, 15f);

		CameraMovementRange.vector2Value = new Vector2 (minVal, maxVal);

		//Save modified changes
		serializedObject.ApplyModifiedProperties();
	}
}
