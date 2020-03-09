using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracker : MonoBehaviour {

	Rocket rocket;
	HUD hud;

	public float totalScore;

	// for counting flips
	float flipStartAngle;
	bool isFlippingClockwise;
	bool isFlippingCounterClockwise;
	float degreesFlippedFramePrior = 0;
	float degreesFlipped = 0;
	string flipDirection;
	int flipsTotal = 0;
	int flipsThisRotation = 0;

	// for counting burns
	bool isBurning;
	float burnStartTime;
	int burnStreak = 0; // in seconds

  // Start is called before the first frame update
  void Start() {
		rocket = FindObjectOfType<Rocket>();
		hud = FindObjectOfType<HUD>();
  }

  // Update is called once per frame
  void Update() {
		if (rocket.state == Rocket.State.Alive) {
			TrackRotation();
			TrackBurnTime();
		}
  }

	public void AddLandingScore() {
		totalScore += 10000f;

		hud.QueueNewScore("Touchdown!", 10000f);
	}

	public void AddEnemyDestroyedScore() {
		totalScore += 5000f;

		hud.QueueNewScore("Enemy Destroyed!", 5000f);
	}

	private void TrackBurnTime() {

		if (Input.GetKeyDown(KeyCode.Space)) {
			burnStartTime = Time.time;
			isBurning = true;
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			isBurning = false;
			burnStreak = 0;
		}

		if ((Time.time - burnStartTime) - burnStreak >= 1f && isBurning) {
			burnStreak++;
			totalScore += burnStreak * 100;
			hud.QueueNewScore(burnStreak + "s BOOST!", burnStreak * 1000f);
		}
	}

	private void TrackRotation() {

		if (Input.GetKeyUp(KeyCode.D)) {

			isFlippingClockwise = false;

			if (flipsThisRotation > 0) {
				flipsTotal += flipsThisRotation;
				print("this rotation" + flipsThisRotation);
			}

			flipsThisRotation = 0;
		}

		if (Input.GetKeyUp(KeyCode.A)) {

			isFlippingCounterClockwise = false;

			if (flipsThisRotation > 0) {
				flipsTotal += flipsThisRotation;
			}

			flipsThisRotation = 0;
		}


		if (Input.GetKeyDown(KeyCode.D)) {
			isFlippingClockwise = true;
			isFlippingCounterClockwise = false;
			flipDirection = "clockwise";
			flipStartAngle = transform.eulerAngles.z;
			degreesFlipped = 0;
			degreesFlippedFramePrior = 0;
			if (flipStartAngle < 180f) {
				flipsThisRotation = -1;
			} else {
				flipsThisRotation = 0;
			}
		}

		if (Input.GetKeyDown(KeyCode.A)) {
			isFlippingClockwise = false;
			isFlippingCounterClockwise = true;
			flipDirection = "counterClockwise";
			flipStartAngle = transform.eulerAngles.z;
			degreesFlipped = 0;
			degreesFlippedFramePrior = 0;
			if (flipStartAngle > 180f) {
				flipsThisRotation = -1;
			} else {
				flipsThisRotation = 0;
			}
		}

		if (isFlippingClockwise) {

			degreesFlippedFramePrior = degreesFlipped;
				
			degreesFlipped = flipStartAngle - transform.eulerAngles.z;
				
			if (degreesFlipped < degreesFlippedFramePrior) {
				flipsThisRotation++;
				if (flipsThisRotation > 0) {
					hud.QueueNewScore(flipsThisRotation + "x FLIP!", flipsThisRotation * 1000f);
					print("flips: " + flipsThisRotation);
				}
			}
		}

		if (isFlippingCounterClockwise) {

			degreesFlippedFramePrior = degreesFlipped;

			degreesFlipped = transform.eulerAngles.z - flipStartAngle;

			if (degreesFlipped < degreesFlippedFramePrior) {
				flipsThisRotation++;
				if (flipsThisRotation > 0) {
					hud.QueueNewScore(flipsThisRotation + "x FLIP!", flipsThisRotation * 1000f);
					print("flips: " + flipsThisRotation);
				}
			}
		}
	}

	public float GetBaseScore() {


		return totalScore;
	}
}
