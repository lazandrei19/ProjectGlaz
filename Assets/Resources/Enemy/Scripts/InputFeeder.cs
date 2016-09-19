using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InputFeeder {
	List <char> chars = new List <char> ();
	int index = 0;

	public InputFeeder (string feed = " ") {
		if (feed.Count () > 0) {
			this.chars = Regex.Replace (feed.ToUpperInvariant (), ".", " $0").ToCharArray ().ToList ();
		}
	}

	public bool IsNull {
		get {
			return chars.Count (c => c != ' ') == 0;
		}
	}

	public bool HasLeft {
		get {
			return index < chars.Count;
		}
	}

	public char Next {
		get {
			if (!HasLeft) {
				index = 0;
			}
			return chars [index++];
		}
	}
}