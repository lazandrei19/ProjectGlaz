using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	bool isMenuVisible = false;
	bool menuHasBeenToggled = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			if (!menuHasBeenToggled) {
				menuHasBeenToggled = true;
				isMenuVisible = !isMenuVisible;
				if (isMenuVisible) {
					Time.timeScale = 0f;
					System.GC.Collect ();
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				} else {
					Time.timeScale = 1f;
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
			}
		} else {
			menuHasBeenToggled = false;
		}
	}

	void OnGUI () {
		if (isMenuVisible) {
			if (GUI.Button (new Rect (50, 50, 200, 100), "Restart")) {
				Time.timeScale = 1f;
				Scene scene = SceneManager.GetActiveScene(); 
				SceneManager.LoadScene(scene.name);
			}

			if (GUI.Button (new Rect (50, 160, 200, 100), "Quit")) {
				Application.Quit ();
			}
		}
	}
}
