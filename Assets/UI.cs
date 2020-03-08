using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI : MonoBehaviour {

	bool paused = false;

	[SerializeField] Canvas menu;

  // Start is called before the first frame update
  void Start() {
		menu.enabled = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update() {
		RunPauseControls();
		HandleMenuDisplay();
  }


	private void HandleMenuDisplay() {
		if (Time.timeScale == 0) {
			menu.enabled = true;
			//Time.timeScale = 0;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		} else {
			menu.enabled = false;
			//Time.timeScale = 1;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void LoadTitleScreen() {
		print("dont look at me");
		//paused = false;
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void Resume() {
		print("hmm");
		Time.timeScale = 1;
		menu.enabled = false;
	}

	private void RunPauseControls() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (Time.timeScale == 1) {
				Time.timeScale = 0;
			} else {
				Time.timeScale = 1;
			}

			//			if (paused) {
			//	paused = false;
			//} else {
			//	paused = true;
			//}
		}
	}
}
