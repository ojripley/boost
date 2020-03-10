using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

	TMPro.TextMeshProUGUI timeText;
	TMPro.TextMeshProUGUI scoreType;
	TMPro.TextMeshProUGUI scoreValue;

	float startTime = 0;
	float finalTime = 0f;

	List<string> queuedScoreTypes= new List<string>();
	List<float> queuedScoreValues = new List<float>();

	float timeSinceDisplayedScore = 0;

	// Start is called before the first frame update
	void Start() {
		startTime = 0;
		Transform timer = gameObject.transform.GetChild(0);
		Transform sT = gameObject.transform.GetChild(1);
		Transform sV = gameObject.transform.GetChild(2);
		timeText = timer.GetComponent<TMPro.TextMeshProUGUI>();
		scoreType = sT.GetComponent<TMPro.TextMeshProUGUI>();
		scoreValue = sV.GetComponent<TMPro.TextMeshProUGUI>();

		scoreType.text = "";
		scoreValue.text = "";
	}

	// Update is called once per frame
	void Update() {
		if (startTime > 0) {

			if (finalTime == 0) {
				float roundedTime = Mathf.Round((Time.time - startTime) * 100) / 100;
				timeText.text = roundedTime.ToString();
			} else {
				timeText.text = finalTime.ToString();
			}

		}

		if (queuedScoreTypes.Count > 0) {
			DisplayNewScore();
		}
	}

	public void BeginCounting() {
		startTime = Time.time;
	}

	public void DisplayNewScore() {
		if (queuedScoreValues.Count > 0 && Time.time - timeSinceDisplayedScore > 1f) {

			string typeToDisplay = queuedScoreTypes[queuedScoreTypes.Count - 1].ToString();
			float valueToDisplay = (float) queuedScoreValues[queuedScoreValues.Count - 1];

			scoreType.text = queuedScoreTypes[0];
			scoreValue.text = "+" + queuedScoreValues[0].ToString();
			timeSinceDisplayedScore = Time.time;

			if (queuedScoreTypes.Count > 0) {
				queuedScoreTypes.RemoveAt(0);
				queuedScoreValues.RemoveAt(0);
			};

			if (queuedScoreTypes.Count == 0) {
				Invoke("RemoveScoreDisplay", 1f);
			} 
		}
	}

	public void StopTimer() {
		float roundedTime = Mathf.Round((Time.time - startTime) * 100) / 100;
		finalTime = roundedTime;
		print(finalTime);
	}

	public void QueueNewScore(string type, float value) {
		queuedScoreTypes.Add(type);
		queuedScoreValues.Add(value);
	}

	public void RemoveScoreDisplay() {
		if (queuedScoreTypes.Count == 0) { // make sure no scores have been added since Invoke()
			scoreType.text = "";
			scoreValue.text = "";
		}
	}

	public float GetFinalTime() {

		StopTimer();

		print("i am returning " + finalTime);

		return finalTime;
	}
}

