using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerDrawer : Editor {

	//All fields from PlayerController
	SerializedProperty CameraReference;
	SerializedProperty PivotReference;
	SerializedProperty EyesReference;
	SerializedProperty BackEyesReference;
	SerializedProperty RotateAmount;
	SerializedProperty CameraMovementRange;
	SerializedProperty CameraRotationRange;
	SerializedProperty CameraCurve;
	SerializedProperty NumberOfTilesToMove;
	SerializedProperty MovementAnimationSpeed;
	SerializedProperty RotationAnimationSpeed;

	//Helpers
	float minCMRVal, minCRRVal, maxCMRVal, maxCRRVal;

	void OnEnable () {
		//Initialization of all property fields
		CameraReference = serializedObject.FindProperty ("er_CameraTransform");
		PivotReference = serializedObject.FindProperty ("er_PivotTransform");
		EyesReference = serializedObject.FindProperty ("er_EyesTransform");
		BackEyesReference = serializedObject.FindProperty ("er_BackEyesTransform");
		RotateAmount = serializedObject.FindProperty ("e_RotateAmount");
		CameraMovementRange = serializedObject.FindProperty ("e_CameraMovementRange");
		CameraRotationRange = serializedObject.FindProperty ("e_CameraRotationRange");
		CameraCurve = serializedObject.FindProperty ("e_CameraClippingCurve");
		NumberOfTilesToMove = serializedObject.FindProperty ("e_NumberOfTilesToMove");
		MovementAnimationSpeed = serializedObject.FindProperty ("e_MovementDuration");
		RotationAnimationSpeed = serializedObject.FindProperty ("e_RotationDuration");
		minCMRVal = CameraMovementRange.vector2Value.x;
		minCRRVal = CameraRotationRange.vector2Value.x;
		maxCMRVal = CameraMovementRange.vector2Value.y;
		maxCRRVal = CameraRotationRange.vector2Value.y;
	}

	public override void OnInspectorGUI() {
		//Update's the serialized object representation to be able to undo
		serializedObject.Update();

		//Field
		EditorGUILayout.PropertyField (CameraReference, new GUIContent ("Camera Reference", "A reference to the Camera that follows the player"));
		EditorGUILayout.PropertyField (PivotReference, new GUIContent ("Pivot Reference", "A reference to the Pivot that allows the camera to rotate around the X-axis"));
		EditorGUILayout.PropertyField (EyesReference, new GUIContent ("Eyes Reference", "A reference to the eyes of the player. Used for raycasting and determining if movement is possible"));
		EditorGUILayout.PropertyField (BackEyesReference, new GUIContent ("Back eyes Reference", "A reference to the point from which raycasts are shoot towards the camera to prevent clipping"));

		EditorGUILayout.Space ();
		RotateAmount.floatValue = EditorGUILayout.IntSlider (new GUIContent ("Rotate amount", "The amount of degrees to rotate when pressing the rotate button once"), Mathf.RoundToInt (RotateAmount.floatValue), 1, 360);

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (new GUIContent ("Camera movement range", "The values between which the camera moves when trying to avoid clipping"));
		minCMRVal = EditorGUILayout.FloatField ("", minCMRVal, GUILayout.MaxWidth (80f));
		maxCMRVal = EditorGUILayout.FloatField ("", maxCMRVal, GUILayout.MaxWidth (80f));
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.MinMaxSlider (new GUIContent (""), ref minCMRVal, ref maxCMRVal, -15f, 15f);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField (new GUIContent ("Camera rotation range", "The values between which the camera rotates when trying to avoid clipping"));
		minCRRVal = EditorGUILayout.FloatField ("", minCRRVal, GUILayout.MaxWidth (80f));
		maxCRRVal = EditorGUILayout.FloatField ("", maxCRRVal, GUILayout.MaxWidth (80f));
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.MinMaxSlider (new GUIContent (""), ref minCRRVal, ref maxCRRVal, 0f, 180f);
		CameraCurve.animationCurveValue = EditorGUILayout.CurveField (new GUIContent ("Camera clipping curve", "The curve which the camera follows for the clipping process"), CameraCurve.animationCurveValue, Color.red, new Rect (0f, 0f, 1f, 1f));


		EditorGUILayout.Space ();
		NumberOfTilesToMove.intValue = EditorGUILayout.IntField (new GUIContent ("Number of tiles to move", "The niumber of tiles to move when pressing the move button once"), NumberOfTilesToMove.intValue);
		MovementAnimationSpeed.floatValue = EditorGUILayout.Slider (new GUIContent ("Movement animation speed", "The duration (in seconds) of moving once"), MovementAnimationSpeed.floatValue, .001f, 1f);
		RotationAnimationSpeed.floatValue = EditorGUILayout.Slider (new GUIContent ("Rotation animation speed", "The duration (in seconds) of rotating once"), RotationAnimationSpeed.floatValue, .001f, 1f);

		CameraMovementRange.vector2Value = new Vector2 (minCMRVal, maxCMRVal);
		CameraRotationRange.vector2Value = new Vector2 (minCRRVal, maxCRRVal);

		//Save modified changes
		serializedObject.ApplyModifiedProperties();
	}
}
