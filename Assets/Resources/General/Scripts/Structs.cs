using System;
using System.Linq;
using UnityEngine;
using System.Collections;

public class Structs {
	//structs used for easier animation
	[Serializable]
	public struct MovementAnimationMetadata {
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

	[Serializable]
	public struct RotationAnimationMetadata {
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

	[Serializable]
	public struct ClippingAnimationMetadata {
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

	[Serializable]
	public struct EnemyAction {
		public EnemyActionType eat;
		public Direction direction;
		public int ammount;

		public string AsString {
			get {
				switch (eat) {
				case EnemyActionType.MOVE:
					switch (direction) {
					case Direction.FORWARD:
						return new String ('w', ammount);
					case Direction.BACKWARD:
						return new String ('s', ammount);
					case Direction.RIGHT:
						return new String ('d', ammount);
					case Direction.LEFT:
						return new String ('a', ammount);
					default:
						return "";
					}
				case EnemyActionType.ROTATE:
					switch (direction) {
					case Direction.RIGHT:
						return new String ('e', ammount);
					case Direction.LEFT:
						return new String ('q', ammount);
					default:
						return "";
					}
				default:
					return "";
				}
			}
		}

		public bool IsGood {
			get {
				//Logic
				if (eat == EnemyActionType.MOVE) {
					if (direction == Direction.UP || direction == Direction.DOWN) {
						return false;
					} else {
						return true;
					}
				} else if (eat == EnemyActionType.ROTATE) {
					if (direction == Direction.RIGHT || direction == Direction.LEFT) {
						return true;
					} else {
						return false;
					}
				} else {
					return false;
				}
			}
		}

		public EnemyAction (EnemyActionType eat, Direction dir, int amm) {
			this.eat = eat;
			this.direction = dir;
			this.ammount = (amm > 0)? amm : 1;
		}
	}
}
