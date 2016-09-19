using UnityEngine;
using ExtensionHelper;
using System.Collections;


public class EnemyController : MonoBehaviour {
	#region public variables
	//Editor modifiable variables
	public float e_RotateAmount = 45f;									//The amount by which the player rotates when one of the rotation keys is pressed

	public Transform er_EyesTransform;									//The Transform of the eyes

	public int e_NumberOfTilesToMove = 2;								//The number of tiles to jump when moving once

	public float e_MovementDuration = .1f;								//The duration (in seconds) of moving once
	public float e_RotationDuration = .1f;								//The duration (in seconds) of rotating once

	public IEnemyBrain enemyBrain;												//The default AI to use
	#endregion

	#region private variables
	//private vars used for code speedup
	bool shouldRotate = true;
	bool shouldMove = true;
	bool isOnDiag = false;
	bool working = false;
	Structs.MovementAnimationMetadata movementMetadata = new Structs.MovementAnimationMetadata (0f, Vector3.zero, 0f);
	Structs.RotationAnimationMetadata rotationMetadata = new Structs.RotationAnimationMetadata (0f, 0);
	InputFeeder inputFeeder;
	char input = ' ';
	#endregion

	#region properties
	//private properties used for easier code reading
	int RotateDirection {
		get {
			if (input == 'Q') {
				return -1;
			} else if (input == 'E') {
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
			if (input == 'W') {
				return Vector3.forward;
			} else if (input == 'A') {
				return Vector3.left;
			} else if (input == 'S') {
				return Vector3.back;
			} else if (input == 'D') {
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
	}

	void Start () {
		EnemyManager.Register (this);
		inputFeeder = new InputFeeder ();
	}

	#region move player
	void MovePlayer () {
		if (movementMetadata.isAnimating) {
			float progress = Time.fixedDeltaTime / movementMetadata.length;
			if (movementMetadata.progress + progress >= 1f) {
				progress = 1f - movementMetadata.progress;
				movementMetadata = new Structs.MovementAnimationMetadata (0f, Vector3.zero, 0f);

				EnemyManager.Done (this);
			}
			movementMetadata.progress += progress;
			transform.Translate (movementMetadata.moveDirection * movementMetadata.moveAmount * e_NumberOfTilesToMove * progress);
			return;
		}

		if (GameController.gc.Get <GameloopController> ("Managers/GLC").turn != Turn.ENEMY) {
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

		movementMetadata = new Structs.MovementAnimationMetadata (e_MovementDuration, MoveDirection, MoveAmount);
		shouldMove = false;
	}
	#endregion

	#region rotate player
	void RotatePlayer () {
		if (rotationMetadata.isAnimating) {
			float progress = Time.deltaTime / rotationMetadata.length;
			if (rotationMetadata.progress + progress >= 1f) {
				progress = 1f - rotationMetadata.progress;
				rotationMetadata = new Structs.RotationAnimationMetadata (0f, 0);
			}
			rotationMetadata.progress += progress;
			transform.Rotate (Vector3.up, e_RotateAmount * rotationMetadata.rotationDirection * progress);
			return;
		}

		if (GameController.gc.Get <GameloopController> ("Managers/GLC").turn != Turn.ENEMY) {
			return;
		}

		if (RotateDirection == 0) {
			shouldRotate = true;
			return;
		}

		if (!shouldRotate) {
			return;
		}

		rotationMetadata = new Structs.RotationAnimationMetadata (e_RotationDuration, RotateDirection);
		shouldRotate = false;
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

	#region enemy specific functions
	public void Think () {
		inputFeeder = enemyBrain.Think (this, inputFeeder);
		working = true;
	}

	public void PrepareForNextThink () {
		working = false;
	}
	#endregion

	void FixedUpdate () {
		if (!working) {
			return;
		}

		MovePlayer ();
		RotatePlayer ();
		FixPosition ();

		if (movementMetadata.isAnimating || rotationMetadata.isAnimating) {
			return;
		}

		input = inputFeeder.Next;
	}

	//Activates on "Reset component"
	void Reset () {
		e_RotateAmount = 45f;
		e_NumberOfTilesToMove = 2;
	}
}
