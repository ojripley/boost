using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour {

	Rocket rocket;
	public float score;

	// for counting flips
	float flipStartAngle;
	bool isFlipping;
	float degreesFlippedFramePrior = 0;
	float degreesFlipped = 0;
	string flipDirection;
	int flipsTotal = 0;
	int flipsThisRotation = 0;

	// for counting burns
	float burnStartTime;
	float burnEndTime;

  // Start is called before the first frame update
  void Start() {
		rocket = FindObjectOfType<Rocket>();
  }

  // Update is called once per frame
  void Update() {
		if (rocket.state == Rocket.State.Alive) {
			TrackRotation();
			TrackBurnTime();
		}
  }

	private void TrackBurnTime() {
		if (Input.GetKey(KeyCode.Space)) {
			burnStartTime = Time.time;
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			burnEndTime = Time.time;
		}

		if (burnEndTime - burnStartTime % 1000 > 1) { // if time of burn is longer than a second
		  // todo hud.addBurnScore(time);

			print(burnEndTime - burnStartTime);
		}
	}

	private void TrackRotation() {

		if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) {

			isFlipping = false;
			if (flipsThisRotation > 0) {
				flipsTotal += flipsThisRotation;
			}

			flipsThisRotation = 0;
		}

		if (isFlipping) {

			degreesFlippedFramePrior = degreesFlipped;

			if (flipDirection == "counterClockwise") {
				
				degreesFlipped = transform.eulerAngles.z - flipStartAngle;
			} else if (flipDirection == "clockwise") {
				degreesFlipped = flipStartAngle - transform.eulerAngles.z;
			}
				
			if (degreesFlipped < degreesFlippedFramePrior) {

				flipsThisRotation++;
			}
		}

		if (Input.GetKeyDown(KeyCode.D)) {
			isFlipping = true;
			flipDirection = "clockwise";
			flipStartAngle = transform.eulerAngles.z;
			if (flipStartAngle < 180f) {
				flipsThisRotation = -1;
			} else {
				flipsThisRotation = 0;
			}
		}

		if (Input.GetKeyDown(KeyCode.A)) {
			isFlipping = true;
			flipDirection = "counterClockwise";
			flipStartAngle = transform.eulerAngles.z;
			if (flipStartAngle > 180f) {
				flipsThisRotation = -1;
			} else {
				flipsThisRotation = 0;
			}
		}
	}
}
