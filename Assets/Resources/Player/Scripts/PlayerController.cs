using UnityEngine;
using ExtensionHelper;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public Transform er_CameraTransform;								//The Transform of the camera
	public Vector2 e_CameraMovementRange = new Vector2 (-6f, 3f);		//The range in which the camera is allowed to move on the Z-axis

	public float e_RotateAmount = 45f;									//The amount by which the player rotates when one of the rotation keys is pressed

	public Transform er_EyesTransform;									//The Transform of the eyes
	public Transform er_BackEyesTransform;								//The point from where the backwards raycast shoots towards the camera

	public int e_NumberOfTilesToMove = 2;								//The number of tiles to jump when moving once

	RaycastHit cameraHit;

	bool shouldRotate = true;
	bool shouldMove = true;
	bool isOnDiag = false;
	bool cameraClippingDirty = false;

	float maxCameraRaycast;

	int RotateDirection {
		get {
			if (Input.GetKey (KeyCode.Q)) {
				return -1;
			} else if (Input.GetKey (KeyCode.E)) {
				return 1;
			} else {
				return 0;
			}
		}
	}

	float MoveAmount {
		get {
			if (isOnDiag) {
				return Mathf.Sqrt (2f);
			} else {
				return 1f;
			}
		}
	}

	Vector3 MoveDirection {
		get {
			if (Input.GetKey (KeyCode.W)) {
				return Vector3.forward;
			} else if (Input.GetKey (KeyCode.A)) {
				return Vector3.left;
			} else if (Input.GetKey (KeyCode.S)) {
				return Vector3.back;
			} else if (Input.GetKey (KeyCode.D)) {
				return Vector3.right;
			} else {
				return Vector3.zero;
			}
		}
	}

	void Awake () {
		// Hide the cursor and lock it in the middle
		Cursor.lockState = CursorLockMode.Locked;
		// Calculates the maximum practical distance for the raycast
		maxCameraRaycast = Mathf.Max (Mathf.Abs (e_CameraMovementRange.x), Mathf.Abs (e_CameraMovementRange.y));
	}

	void Start () {
		
	}

	void MovePlayer () {
		if (MoveDirection == Vector3.zero) {
			shouldMove = true;
			return;
		}

		if (!shouldMove) {
			return;
		}

		if (Physics.Raycast (er_EyesTransform.position, transform.TransformDirection (MoveDirection), MoveAmount * e_NumberOfTilesToMove - er_EyesTransform.localPosition.z)) {
			return;
		}

		if (MoveDirection == Vector3.back) {
			if (Physics.Raycast (er_EyesTransform.position, transform.TransformDirection (MoveDirection), MoveAmount * e_NumberOfTilesToMove - er_EyesTransform.localPosition.z + 1)) {
				return;
			}
		}

		transform.Translate (MoveDirection * MoveAmount * e_NumberOfTilesToMove);
		shouldMove = false;
		cameraClippingDirty = true;
	}

	void PreventCameraClipping () {
		
		if (!cameraClippingDirty) {
			return;
		}

		er_CameraTransform.localPosition = new Vector3 (0, 1, e_CameraMovementRange.x);
		er_CameraTransform.localEulerAngles = new Vector3 (30, 0, 0);

		if (!Physics.Raycast (er_BackEyesTransform.position, er_CameraTransform.TransformDirection (Vector3.back), out cameraHit, maxCameraRaycast)) {
			cameraClippingDirty = false;
			return;
		}

		if (!cameraHit.collider.HasTag ("CameraCollider")) {
			cameraClippingDirty = false;
			return;
		}

		er_CameraTransform.localPosition = new Vector3 (0, 1, e_CameraMovementRange.x + (cameraHit.point - er_CameraTransform.position).magnitude);

		if (er_CameraTransform.localPosition.z > -2f && er_CameraTransform.localPosition.z < 2.5f) {
			er_CameraTransform.localPosition = new Vector3 (0f, 1f, 3f);
		}

		if (er_CameraTransform.localPosition.z > -2f) {
			er_CameraTransform.localEulerAngles = new Vector3 (30, 180, 0);
		}

		cameraClippingDirty = false;
	}

	void RotatePlayer () {
		if (RotateDirection == 0) {
			shouldRotate = true;
			return;
		}

		if (!shouldRotate) {
			return;
		}

		transform.Rotate (Vector3.up, e_RotateAmount * RotateDirection);
		shouldRotate = false;
		cameraClippingDirty = true;
		isOnDiag = !isOnDiag;
	}

	void FixPosition () {
		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z));
		transform.eulerAngles = new Vector3 (Mathf.Round (transform.eulerAngles.x), Mathf.Round (transform.eulerAngles.y), Mathf.Round (transform.eulerAngles.z));
	}

	//Activates on "Reset component"
	void Reset () {
		er_CameraTransform = Camera.main.transform;
		e_CameraMovementRange = new Vector2 (-6f, 3f);
		e_RotateAmount = 45f;
		e_NumberOfTilesToMove = 2;
	}

	void FixedUpdate () {
		MovePlayer ();
		RotatePlayer ();
		FixPosition ();
		PreventCameraClipping ();
	}
}
