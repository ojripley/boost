using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;

	// '[SerializeField]' allows variables to be changed from the Unity Inspector
	[SerializeField] float progradeThrustMulitplier = 20f;
	[SerializeField] float rotationMultiplier = 400f;

	// Start is called before the first frame update
	void Start() {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update() {
		handleInput();
	}

	private void handleInput() {

		rigidBody.freezeRotation = true; // disallows external control, prevents physics induced spin

		// can always thrust, even while rotating
		if (Input.GetKey(KeyCode.Space)) {


			rigidBody.AddRelativeForce(Vector3.up * progradeThrustMulitplier);
			handleAudio();
		}

		// stop playing when space is released
		if (Input.GetKeyUp(KeyCode.Space)) {
			audioSource.Stop();
		}

		if (Input.GetKey(KeyCode.Q)) {
			transform.Rotate(Vector3.forward * rotationMultiplier * Time.deltaTime); // left handed coordinate system in the z-axis. this means +ve values go counter-clockwise
		} else if (Input.GetKey(KeyCode.E)) {
			transform.Rotate(Vector3.back * rotationMultiplier * Time.deltaTime);
		}

		rigidBody.freezeRotation = false; // once player control is released, reallow physics induced spin
	}


	// handles audio on thrust
	private void handleAudio() {
		if (!audioSource.isPlaying) { // prevents starting the clip on every frame;
			audioSource.Play();
		}

		// stop playing when space is released
		if (Input.GetKeyUp(KeyCode.Space)) {
			audioSource.Stop();
		}
	}
}