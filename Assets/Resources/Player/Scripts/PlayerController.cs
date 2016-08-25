using UnityEngine;
using ExtensionHelper;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public Transform er_CameraTransform;								//The Transform of the camera
	public Vector2 e_CameraMovementRange = new Vector2 (-6f, -0.5f);	//The range in which the camera is allowed to move on the Z-axis

	public float e_RotateAmount = 45f;									//The amount by which the player rotates when one of the rotation keys is pressed

	public Transform er_EyesTransform;									//The Transform of the eyes

	RaycastHit cameraHit;

	bool shouldRotate = true;
	bool shouldMove = true;
	bool isOnDiag = false;
	bool cameraClippingDirty = false;

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
		//Hide the cursor and lock it in the middle
		Cursor.lockState = CursorLockMode.Locked;
	}

	void MovePlayer () {
		if (MoveDirection == Vector3.zero) {
			shouldMove = true;
			return;
		}

		if (!shouldMove) {
			return;
		}

		if (Physics.Raycast (er_EyesTransform.position, transform.TransformDirection (MoveDirection), MoveAmount - er_EyesTransform.localPosition.z)) {
			return;
		}

		transform.Translate (MoveDirection * MoveAmount);
		shouldMove = false;
		cameraClippingDirty = true;
	}

	void PreventCameraClipping () {
		if (!cameraClippingDirty) {
			return;
		}

		er_CameraTransform.localPosition = new Vector3 (0, 1, e_CameraMovementRange.x);

		if (!Physics.Raycast (er_CameraTransform.position, er_CameraTransform.TransformDirection (Vector3.forward), out cameraHit, 5f)) {
			return;
		}

		if (!cameraHit.collider.HasTag ("CameraCollider")) {
			return;
		}

		er_CameraTransform.localPosition = new Vector3 (0, 1, Mathf.Clamp(e_CameraMovementRange.x + (cameraHit.point - er_CameraTransform.position).magnitude, e_CameraMovementRange.x, e_CameraMovementRange.y));
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
		e_CameraMovementRange = new Vector2 (-6f, -0.5f);
	}

	void FixedUpdate () {
		MovePlayer ();
		RotatePlayer ();
		FixPosition ();
		PreventCameraClipping ();
	}
}
