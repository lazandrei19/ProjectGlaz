using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerDrawer : Editor {

	//All fields from PlayerController
	SerializedProperty MouseLimits;
	SerializedProperty CameraRotationSpeedHorizontal;
	SerializedProperty CameraRotationSpeedVertical;
	SerializedProperty MaxHorizontalRotation;
	SerializedProperty MaxVerticalRotation;
	SerializedProperty VerticalRotationRange;
	SerializedProperty CameraPivot;
	SerializedProperty CameraReference;
	SerializedProperty InvertHMovement;
	SerializedProperty InvertVMovement;
	SerializedProperty CameraMovementLimits;

	SerializedProperty MoveSpeed;
	SerializedProperty RunningSpeed;

	bool IHM; //Bool version for InvertHMovement
	bool IVM; //Bool version for InvertVMovement

	float CameraMovementMin, CameraMovementMax;

	void OnEnable () {
		//Initialization of all property fields
		InvertHMovement = serializedObject.FindProperty ("e_InvertHMovement");
		InvertVMovement = serializedObject.FindProperty ("e_InvertVMovement");
		MouseLimits = serializedObject.FindProperty ("e_MouseLimits");
		CameraRotationSpeedHorizontal = serializedObject.FindProperty ("e_LookRotationSpeedHorizontal");
		CameraRotationSpeedVertical = serializedObject.FindProperty ("e_LookRotationSpeedVertical");
		MaxHorizontalRotation = serializedObject.FindProperty ("e_MaxHorizontalRotation");
		MaxVerticalRotation = serializedObject.FindProperty ("e_MaxVerticalRotation");
		VerticalRotationRange = serializedObject.FindProperty ("e_MaxVerticalRotationRange");
		CameraPivot = serializedObject.FindProperty ("er_CameraPivot");

		CameraReference = serializedObject.FindProperty ("er_CameraTransform");
		CameraMovementLimits = serializedObject.FindProperty ("e_CameraMovementRange");

		MoveSpeed = serializedObject.FindProperty ("e_MoveSpeed");
		RunningSpeed = serializedObject.FindProperty ("e_RunningSpeed");

		//Initialization of aditional fields
		IHM = InvertHMovement.floatValue == -1f;
		IVM = InvertVMovement.floatValue == 1f;
		CameraMovementMin = CameraMovementLimits.vector2Value.x;
		CameraMovementMax = CameraMovementLimits.vector2Value.y;
	}

	public override void OnInspectorGUI() {
		//Update's the serialized object representation to be able to undo
		serializedObject.Update();

		//Mouse Limits field
		MouseLimits.floatValue = EditorGUILayout.FloatField (new GUIContent ("Mouse Limits", "How much the user has to move the mouse so that it moves the maximum rotation specified"), MouseLimits.floatValue);

		//Camera rotation speed fields
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField ("Camera Rotation Speed", EditorStyles.boldLabel);
		CameraRotationSpeedHorizontal.floatValue = EditorGUILayout.Slider ("Horizontal", CameraRotationSpeedHorizontal.floatValue, 0f, 5f);
		CameraRotationSpeedVertical.floatValue = EditorGUILayout.Slider ("Vertical", CameraRotationSpeedVertical.floatValue, 0f, 5f);

		//Max rotation speed fields
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField (new GUIContent ("Max Rotation", "Maximum rotation on mouse rotation"), EditorStyles.boldLabel);
		MaxHorizontalRotation.floatValue = EditorGUILayout.Slider ("Horizontal", MaxHorizontalRotation.floatValue, 0f, 180f);
		MaxVerticalRotation.floatValue = EditorGUILayout.Slider ("Vertical", MaxVerticalRotation.floatValue, 0f, 90f);

		//Vertical rotation range speed
		EditorGUILayout.Space ();
		VerticalRotationRange.floatValue = EditorGUILayout.Slider ("Vertical Rotation Range", VerticalRotationRange.floatValue, 0f, 90f);

		//Movement speed
		EditorGUILayout.Space ();
		MoveSpeed.floatValue = EditorGUILayout.FloatField (new GUIContent ("Movement speed"), MoveSpeed.floatValue);
		RunningSpeed.floatValue = EditorGUILayout.FloatField (new GUIContent ("Running speed"), RunningSpeed.floatValue);

		//Camera inversion fields
		EditorGUILayout.Space ();
		IHM = EditorGUILayout.Toggle ("Invert Horizontal Camera Movement", IHM);
		IVM = EditorGUILayout.Toggle ("Invert Vertical Camera Movement", IVM);

		//Camera pivot field
		EditorGUILayout.Space ();
		EditorGUILayout.PropertyField (CameraPivot, new GUIContent ("Camera Pivot", "The game object around which the camera pivots on the X-axis"));
		EditorGUILayout.PropertyField (CameraReference, new GUIContent ("Camera Reference", "A reference to the Camera that follows the player"));

		//Camera movement limits field
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField (new GUIContent ("Camera movement", "Camera movement on local Z-axis while preventing cliping"), EditorStyles.boldLabel);
		CameraMovementMin = EditorGUILayout.Slider (new GUIContent ("Minimum"), CameraMovementMin, -10f, 2f);
		CameraMovementMax = EditorGUILayout.Slider (new GUIContent ("Maximum"), CameraMovementMax, -10f, 2f);

		//Update Inverse movement variables
		InvertHMovement.floatValue = (IHM) ? -1f : 1f;
		InvertVMovement.floatValue = (IVM) ? 1f : -1f;
		CameraMovementLimits.vector2Value = (CameraMovementMin < CameraMovementMax)? new Vector2 (CameraMovementMin, CameraMovementMax) : new Vector2 (CameraMovementMax, CameraMovementMin);

		//Save modified changes
		serializedObject.ApplyModifiedProperties();
	}
}
