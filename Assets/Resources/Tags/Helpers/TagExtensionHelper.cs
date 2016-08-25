using UnityEngine;
using System.Collections;

namespace ExtensionHelper {
	public static class TagHelper {

		public static bool HasTag (this Component component, string tag) {
			Tags tags = component.GetComponent <Tags> ();
			if (tags != null) {
				return tags.HasTag (tag);
			} else {
				return false;
			}
		}
	}
}