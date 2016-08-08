using UnityEngine;
using ExtensionHelper;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

	public float e_MouseLimits = 15f;								//The limit in which the player moves the maximum amount in one frame
	public float e_LookRotationSpeedHorizontal = .75f;				//Camera horizontal rotation speed
	public float e_LookRotationSpeedVertical = 1f;					//Camera vertical rotation speed
	public float e_MaxHorizontalRotation = 180f;					//Maximum horizontal rotation in one mouse limit
	public float e_MaxVerticalRotation = 45f;						//Maximum vertical rotation in one mouse limit
	public float e_MaxVerticalRotationRange = 25f;					//Maximum range for vertical rotation
	public float e_InvertHMovement = 1f;							//If the horizontal camera movement should be inverted. 1 is false, -1 is true
	public float e_InvertVMovement = -1f;							//If the vertical camera movement should be inverted. 1 is true, -1 is false

	public Transform er_CameraPivot;								//The Transform around which the camera pivots around the x axis
	public Transform er_CameraTransform;							//The Transform of the camera

	public float e_MoveSpeed = 10f;									//Movement speed
	public float e_RunningSpeed = 20f;								//Running speed

	public Vector2 e_CameraMovementRange = new Vector2 (-4.5f, -0.5f);	//The range in which the camera is allowed to move on the Z-axis

	Rigidbody r_Rigidbody;

	RaycastHit hit;

	float Speed {
		get {
			if (Input.GetKey (KeyCode.LeftShift)) {
				return e_RunningSpeed;
			} else {
				return e_MoveSpeed;
			}
		}
	}


	Vector3 MovementVelocity {
		get { 
			return new Vector3 (
				Input.GetAxis ("Horizontal") * Speed,
				r_Rigidbody.velocity.y,
				Input.GetAxis ("Vertical") * Speed
			);
		}
	}

	void Awake () {
		//Hide the cursor and lock it in the middle
		Cursor.lockState = CursorLockMode.Locked;
		//Initialize RigidBody
		r_Rigidbody = GetComponent <Rigidbody> ();
		//Initialize CameraPivot if not already initialized
		er_CameraPivot = (er_CameraPivot == null)? gameObject.GetComponentInParent <Transform> () : er_CameraPivot;
	}

	void MovePlayer () {
		r_Rigidbody.freezeRotation = true;
		r_Rigidbody.velocity = transform.TransformVector (MovementVelocity);
	}

	void RotateCameraHorizontally () {
		r_Rigidbody.freezeRotation = false;
		//Get the mouse movement since last frame
		float h = Input.GetAxis("Mouse X");
		//Clamp it to limits
		h = Mathf.Clamp (h, -e_MouseLimits, e_MouseLimits);
		//Use the limits to find the actual rotation
		float percentH = h / e_MouseLimits;
		float rotationH = percentH * e_MaxHorizontalRotation;
		//Rotate
		transform.Rotate (new Vector3 (0, rotationH * e_InvertHMovement, 0));
		r_Rigidbody.freezeRotation = true;
	}

	void RotateCameraVertically () {
		r_Rigidbody.freezeRotation = false;
		//Get the movement since last frame
		float v = Input.GetAxis("Mouse Y");
		//Clamp it to limits
		v = Mathf.Clamp (v, -e_MouseLimits, e_MouseLimits);
		//Use the limits to find the actual rotation
		v *= e_MaxVerticalRotation / e_MouseLimits;
		//Calculate the final rotation
		Vector3 finalRotation = er_CameraPivot.localRotation.eulerAngles + Vector3.right * v * e_LookRotationSpeedVertical * e_InvertVMovement;
		//Check if out of bounds
		if (AngleHelper.IsInAngleRange(finalRotation.x, -e_MaxVerticalRotationRange, e_MaxVerticalRotationRange)) {
			er_CameraPivot.Rotate (Vector3.right * v * e_LookRotationSpeedVertical * e_InvertVMovement);
		} else {
			//If it is, Clamp it to bounds
			finalRotation.x = AngleHelper.Clamp (finalRotation.x, -e_MaxVerticalRotationRange, e_MaxVerticalRotationRange);
			er_CameraPivot.localEulerAngles = finalRotation;
		}
		r_Rigidbody.freezeRotation = true;
	}

	void PreventCameraClipping () {
		er_CameraTransform.localPosition = new Vector3 (0, 1, e_CameraMovementRange.x);
		if (Physics.Raycast (er_CameraTransform.position, er_CameraTransform.TransformDirection (Vector3.forward), out hit, 5f)) {
			if (hit.collider.HasTag ("CameraCollider")) {
				er_CameraTransform.localPosition = new Vector3 (0, 1, Mathf.Clamp(e_CameraMovementRange.x + (hit.point - er_CameraTransform.position).magnitude, e_CameraMovementRange.x, e_CameraMovementRange.y));
			}
		}
	}

	//Activates on "Reset component"
	void Reset () {
		e_MouseLimits = 15f;
		e_LookRotationSpeedHorizontal = .75f;
		e_LookRotationSpeedVertical = 1f;
		e_MaxHorizontalRotation = 180f;
		e_MaxVerticalRotation = 45f;
		e_MaxVerticalRotationRange = 25f;
		e_InvertHMovement = 1;
		e_InvertVMovement = -1;
		e_MoveSpeed = 10f;
		e_RunningSpeed = 20f;
		er_CameraPivot = GameObject.Find ("Player/Pivot").transform;
		er_CameraTransform = Camera.main.transform;
		e_CameraMovementRange = new Vector2 (-4.5f, -0.5f);
	}

	void FixedUpdate () {
		MovePlayer ();
		RotateCameraHorizontally ();
		RotateCameraVertically ();
		PreventCameraClipping ();
	}
}
