using UnityEngine;
using System.Collections;

public class AngleHelper {

	//Reduces an angle to a value between -360 and 360
	public static float AngleToFirstCircle (float v) {
		return v % 360;
	}

	//Takes an angle, reduces it between 0 and 360. Negative angles are counted down from 360
	public static float NormalizeAngle (float v) {
		float f = AngleToFirstCircle (v);
		return (f >= 0) ? f : (360 + f);
	}

	//Checks if an angle is in range. Supports negative values for min and max
	public static bool IsInAngleRange (float v, float min, float max) {
		float actMin = NormalizeAngle (min);
		float actMax = NormalizeAngle (max);

		if (actMin < actMax) {
			return v > actMin && v < actMax;
		} else {
			return v > actMin || v < actMax;
		}
	}

	//Clamps an angle between min and max. Supports negative values for min and max
	public static float Clamp (float v, float min, float max) {
		//Normalizes min and max
		float actMin = NormalizeAngle (min);
		float actMax = NormalizeAngle (max);

		if (actMin < actMax) {
			//There aren't negative values
			if (v < actMin) {
				return actMin;
			} else if (v > actMax) {
				return actMax;
			} else {
				return v;
			}
		} else {
			//There are negative values

			//Finds the middle
			//For everything between max and middle, clamps to max
			//For everything between min and middle, clamps to min
			float mid = NormalizeAngle (((min + max) / 2) + 180);

			if (v > actMin || v < actMax) {
				return v;
			} else if (v >= actMax && v < mid) {
				return actMax;
			} else {
				return actMin;
			}
		}
	}
}
