using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;

	// Start is called before the first frame update
	void Start() {
		rigidBody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update() {
		HandleInput();
	}

	private void HandleInput() {

		// can always thrust, even while rotating
		if (Input.GetKey(KeyCode.Space)) {
			rigidBody.AddRelativeForce(Vector3.up);
		}

		if (Input.GetKey(KeyCode.Q)) {
			transform.Rotate(Vector3.forward); // left handed coordinate system in the z-axis. this means +ve values go counter-clockwise
		} else if (Input.GetKey(KeyCode.E)) {
			transform.Rotate(Vector3.back);
		}
	}
}