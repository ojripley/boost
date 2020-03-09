using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UI : MonoBehaviour {

	bool paused = false;

	[SerializeField] Canvas pauseMenu;
	[SerializeField] Canvas levelCompleteMenu;

	[SerializeField] TMPro.TextMeshProUGUI finalTimeDisplay;
	[SerializeField] TMPro.TextMeshProUGUI finalScoreDisplay;

	string finalTime;
	string finalScore;

  // Start is called before the first frame update
  void Start() {
		pauseMenu.enabled = false;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	// Update is called once per frame
	void Update() {
		RunPauseControls();
		HandleMenuDisplay();
  }

	public void SetLevelCompleteMenuDisplay(bool tf) {
		levelCompleteMenu.enabled = tf;
	}

	private void HandleMenuDisplay() {
		if (paused && levelCompleteMenu.enabled == false) {
			pauseMenu.enabled = true;
			//Time.timeScale = 0;
			print("turning mouse on");
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		} else if (levelCompleteMenu.enabled == false) {
			pauseMenu.enabled = false;
			//Time.timeScale = 1;
			print("turning mouse off");
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void LoadTitleScreen() {
		print("dont look at me");
		paused = false;
		Time.timeScale = 1;
		SceneManager.LoadScene(0);
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void Resume() {
		print("hmm");
		Time.timeScale = 1;
		pauseMenu.enabled = false;
		paused = false;
	}

	private void RunPauseControls() {
		if (Input.GetKeyDown(KeyCode.Escape) && levelCompleteMenu.enabled == false) {
			if (Time.timeScale == 1) {
				Time.timeScale = 0;
				paused = true;
			} else {
				Time.timeScale = 1;
				paused = false;
			}
		}
	}

	public void LoadNextLevel() {
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		currentSceneIndex++;

		if (currentSceneIndex == SceneManager.sceneCountInBuildSettings) {
			LoadFirstLevel();
		} else {
			SceneManager.LoadScene(currentSceneIndex);
		}
	}

	private void LoadFirstLevel() {
		SceneManager.LoadScene(0);
	}

	public void SetFinalTimeAndScore(float time, float score) {
		finalTimeDisplay.text = "Time: " + time.ToString();
		finalScoreDisplay.text = "Score: " + Mathf.Round(score / (time / 10)).ToString();
	}
}
