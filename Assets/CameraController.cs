using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	GameObject rocket;
	Vector3 constraintLeft;
	Vector3 constraintRight;

	[Header("Movement Settings")]
	[SerializeField] float cameraHorizontalOffset = 10f;
	[SerializeField] float cameraVerticalOffset = 10f;
	[SerializeField] float cameraDepthOffset = 0f;

	Vector3 rocketStartPosition;
	Vector3 cameraStartPosition;

	// Start is called before the first frame update
	void Start() {
		cameraStartPosition = gameObject.transform.position;
		constraintLeft = GameObject.FindGameObjectWithTag("Cam Constraint Left").transform.position;
		constraintRight = GameObject.FindGameObjectWithTag("Cam Constraint Right").transform.position;
		rocket = GameObject.FindGameObjectWithTag("Player");
	}

  // Update is called once per frame
  void Update() {
		MoveCamera();
  }

	private void MoveCamera() {

		Vector3 rocketCurrentPosition = rocket.transform.position;

		float positionX = rocketCurrentPosition.x + cameraHorizontalOffset;
		float positionY = cameraStartPosition.y + cameraVerticalOffset;
		float positionZ = cameraStartPosition.z + cameraDepthOffset;

		positionX = Mathf.Clamp(positionX, constraintLeft.x,  constraintRight.x);

		Vector3 updatedCameraPosition = new Vector3(positionX, positionY, positionZ);

		gameObject.transform.position = updatedCameraPosition;
	}
}
