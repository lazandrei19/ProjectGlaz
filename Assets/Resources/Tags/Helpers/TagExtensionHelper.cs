using UnityEngine;
using System.Collections;

namespace ExtensionHelper {
	public static class TagHelper {

		public static bool HasTag (this Collider collider, string tag) {
			Tags tags = collider.GetComponent <Tags> ();
			if (tags != null) {
				return tags.HasTag (tag);
			} else {
				return false;
			}
		}
	}
}