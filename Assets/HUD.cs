using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

	TMPro.TextMeshProUGUI timeText;

	float startTime;
	string finalTime = "";

	// Start is called before the first frame update
	void Start() {
		startTime = Time.time;
		Transform timer = gameObject.transform.GetChild(0);
		timeText = timer.GetComponent<TMPro.TextMeshProUGUI>();
		print(timeText);
	}

	// Update is called once per frame
	void Update() {

		if (finalTime.Length == 0) {
			float roundedTime = Mathf.Round((Time.time - startTime) * 100) / 100;
			timeText.text = roundedTime.ToString();
		}
	}

	public void GetFinalTime() {
		float roundedTime = Mathf.Round((Time.time - startTime) * 100) / 100;
		finalTime = roundedTime.ToString();
	}
}
