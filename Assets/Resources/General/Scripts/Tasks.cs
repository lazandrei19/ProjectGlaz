using UnityEngine;
using System.Collections;

public class Tasks {

	public static void Initialize () {
		//Initializes the gameloop controller
		GameController.gc.Register ("Managers/GLC", new GameloopController ());
		// Hide the cursor and lock it in the middle
		Cursor.lockState = CursorLockMode.Locked;
	}
}
