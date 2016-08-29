using UnityEngine;
using ExtensionHelper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour {
	#region structs
	//private structs used for easier animation
	struct MovementAnimationMetadata {
		public float length;
		public float progress;
		public bool isAnimating;
		public Vector3 moveDirection;
		public float moveAmount;

		public MovementAnimationMetadata (float l, Vector3 mD, float mA) {
			length = l;
			progress = 0f;
			isAnimating = (l == 0f)? false : true;
			moveDirection = mD;
			moveAmount = mA;
		}
	}

	struct RotationAnimationMetadata {
		public float length;
		public float progress;
		public bool isAnimating;
		public int rotationDirection;

		public RotationAnimationMetadata (float l, int rD) {
			length = l;
			progress = 0f;
			isAnimating = (l == 0f)? false : true;
			rotationDirection = rD;
		}
	}

	struct ClippingAnimationMetadata {
		public float length;
		public float progress;
		public bool isAnimating;
		public float rotation;
		public float distance;

		public ClippingAnimationMetadata (float l, float d, float r) {
			length = l;
			progress = 0f;
			isAnimating = (l == 0f)? false : true;
			rotation = r;
			distance = d;
		}
	}
	#endregion

	#region public variables
	//Editor modifiable variables
	public float e_RotateAmount = 45f;									//The amount by which the player rotates when one of the rotation keys is pressed

	public Transform er_EyesTransform;									//The Transform of the eyes
	public Transform er_BackEyesTransform;								//The point from where the backwards raycast shoots towards the camera

	public int e_NumberOfTilesToMove = 2;								//The number of tiles to jump when moving once

	public float e_MovementDuration = .1f;								//The duration (in seconds) of moving once
	public float e_RotationDuration = .1f;								//The duration (in seconds) of rotating once

	public float e_ClippingDuration = .1f;								//The duration (in seconds) of the clipping animation

	public Transform er_CameraTransform;								//The Transform of the camera
	public Transform er_PivotTransform;									//The Transform of the pivot
	public Vector2 e_CameraMovementRange = new Vector2 (-6f, 0f);		//The range in which the camera is allowed to move on the Z-axis
	public Vector2 e_CameraRotationRange = new Vector2 (30f, 90f);		//The range in which the camera is alowed to rotate around the X-axis
	public AnimationCurve e_CameraClippingCurve = AnimationCurve.Linear (0f, 0f, 1f, 1f);
	#endregion

	#region private variables
	//private vars used for code speedup
	bool shouldRotate = true;
	bool shouldMove = true;
	bool isOnDiag = false;
	bool cameraClippingDirty = false;
	MovementAnimationMetadata movementMetadata = new MovementAnimationMetadata (0f, Vector3.zero, 0f);
	RotationAnimationMetadata rotationMetadata = new RotationAnimationMetadata (0f, 0);
	ClippingAnimationMetadata clippingMetadata = new ClippingAnimationMetadata (0f, 0f, 0f);

	//Private vars used for camera adjustments
	RaycastHit cameraHit;
	float maxCameraRaycast;
	#endregion

	#region properties
	//private properties used for easier code reading
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
	#endregion

	void Awake () {
		// Hide the cursor and lock it in the middle
		Cursor.lockState = CursorLockMode.Locked;
		// Calculates the maximum practical distance for the raycast
		maxCameraRaycast = Mathf.Max (Mathf.Abs (e_CameraMovementRange.x), Mathf.Abs (e_CameraMovementRange.y));
	}

	#region move player
	void MovePlayer () {
		if (movementMetadata.isAnimating) {
			float progress = Time.fixedDeltaTime / movementMetadata.length;
			if (movementMetadata.progress + progress >= 1f) {
				progress = 1f - movementMetadata.progress;
				movementMetadata = new MovementAnimationMetadata (0f, Vector3.zero, 0f);
			}
			movementMetadata.progress += progress;
			transform.Translate (movementMetadata.moveDirection * movementMetadata.moveAmount * e_NumberOfTilesToMove * progress);
			return;
		}

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

		movementMetadata = new MovementAnimationMetadata (e_MovementDuration, MoveDirection, MoveAmount);
		shouldMove = false;
		cameraClippingDirty = true;
	}
	#endregion

	#region rotate player
	void RotatePlayer () {
		if (rotationMetadata.isAnimating) {
			float progress = Time.deltaTime / rotationMetadata.length;
			if (rotationMetadata.progress + progress >= 1f) {
				progress = 1f - rotationMetadata.progress;
				rotationMetadata = new RotationAnimationMetadata (0f, 0);
			}
			rotationMetadata.progress += progress;
			transform.Rotate (Vector3.up, e_RotateAmount * rotationMetadata.rotationDirection * progress);
			return;
		}

		if (RotateDirection == 0) {
			shouldRotate = true;
			return;
		}

		if (!shouldRotate) {
			return;
		}

		rotationMetadata = new RotationAnimationMetadata (e_RotationDuration, RotateDirection);
		shouldRotate = false;
		cameraClippingDirty = true;
		if (e_RotateAmount == 45f) {
			isOnDiag = !isOnDiag;
		}
	}
	#endregion

	#region fix position
	void FixPosition () {
		if (rotationMetadata.isAnimating || movementMetadata.isAnimating) {
			return;
		}

		transform.position = new Vector3 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y), Mathf.Round (transform.position.z));
		transform.eulerAngles = new Vector3 (Mathf.Round (transform.eulerAngles.x), Mathf.Round (transform.eulerAngles.y), Mathf.Round (transform.eulerAngles.z));
	}
	#endregion

	#region prevent camera clipping
	void PreventCameraClipping () {
		if (clippingMetadata.isAnimating) {
			float animProgress = Time.deltaTime / clippingMetadata.length;
			if (clippingMetadata.progress >= .99f) {
				animProgress = 1f - clippingMetadata.progress;
				clippingMetadata = new ClippingAnimationMetadata (0f, 0f, 0f);
			}
			clippingMetadata.progress += animProgress;
			er_CameraTransform.Translate (new Vector3 (0f, 0f, clippingMetadata.distance * animProgress));
			er_PivotTransform.Rotate (new Vector3 (clippingMetadata.rotation * animProgress, 0f, 0f));
			return;
		}

		if (!cameraClippingDirty) {
			return;
		}

		if (movementMetadata.isAnimating || rotationMetadata.isAnimating) {
			return;
		}

		float initialPos = er_CameraTransform.localPosition.z;
		float initialRot = er_PivotTransform.localEulerAngles.x;

		er_CameraTransform.localPosition = new Vector3 (0f, 0f, e_CameraMovementRange.x);
		er_PivotTransform.localEulerAngles = new Vector3 (e_CameraRotationRange.x, 0f, 0f);

		if (!Physics.Raycast (er_BackEyesTransform.position, er_CameraTransform.TransformDirection (Vector3.back), out cameraHit, maxCameraRaycast)) {
			if (initialPos == e_CameraMovementRange.x) {
				cameraClippingDirty = false;
				return;
			}

			//Same complicated math, but put in one line. Will prettyfy
			float aTM = initialPos - e_CameraMovementRange.x;
			float aTR = initialRot - (e_CameraRotationRange.x + e_CameraClippingCurve.Evaluate (0f) * Mathf.Abs(e_CameraRotationRange.x - e_CameraRotationRange.y));

			er_CameraTransform.localPosition = new Vector3 (0f, 0f, initialPos);
			er_PivotTransform.localEulerAngles = new Vector3 (initialRot, 0f, 0f);

			clippingMetadata = new ClippingAnimationMetadata (e_ClippingDuration, -aTM, -aTR);

			cameraClippingDirty = false;
			return;
		}

		if (!cameraHit.collider.HasTag ("CameraCollider")) {
			if (initialPos == e_CameraMovementRange.x) {
				cameraClippingDirty = false;
				return;
			}

			//Same complicated math, but put in one line. Will prettyfy
			float aTM = initialPos - e_CameraMovementRange.x;
			float aTR = initialRot - (e_CameraRotationRange.x + e_CameraClippingCurve.Evaluate (0f) * Mathf.Abs(e_CameraRotationRange.x - e_CameraRotationRange.y));

			er_CameraTransform.localPosition = new Vector3 (0f, 0f, initialPos);
			er_PivotTransform.localEulerAngles = new Vector3 (initialRot, 0f, 0f);

			clippingMetadata = new ClippingAnimationMetadata (e_ClippingDuration, -aTM, -aTR);

			cameraClippingDirty = false;
			return;
		}

		//Complicated math
		float movementDelta = (cameraHit.point - er_CameraTransform.position).magnitude;
		float targetMovePos = e_CameraMovementRange.x + movementDelta;
		float movementRange = Mathf.Abs(e_CameraMovementRange.x - e_CameraMovementRange.y);
		float rotationRange = Mathf.Abs(e_CameraRotationRange.x - e_CameraRotationRange.y);
		float progress = movementDelta / movementRange;
		float rotationDelta = e_CameraClippingCurve.Evaluate (progress) * rotationRange;
		float targetRotatePos = e_CameraRotationRange.x + rotationDelta;

		float ammountToMove = initialPos - targetMovePos;
		float ammountToRotate = initialRot - targetRotatePos;

		er_CameraTransform.localPosition = new Vector3 (0f, 0f, initialPos);
		er_PivotTransform.localEulerAngles = new Vector3 (initialRot, 0f, 0f);

		//I don't know why this is negative. Probably something in my math is off. It works like this. I'll change it later
		clippingMetadata = new ClippingAnimationMetadata (e_ClippingDuration, -ammountToMove, -ammountToRotate);

		cameraClippingDirty = false;
	}
	#endregion

	void FixedUpdate () {
		MovePlayer ();
		RotatePlayer ();
		FixPosition ();
		PreventCameraClipping ();
	}

	//Activates on "Reset component"
	void Reset () {
		er_CameraTransform = Camera.main.transform;
		e_CameraMovementRange = new Vector2 (-6f, 3f);
		e_RotateAmount = 45f;
		e_NumberOfTilesToMove = 2;
	}
}
