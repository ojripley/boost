using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class OscillatingObstacle : MonoBehaviour {

	[SerializeField] Vector3 movementVector;
	[SerializeField] float period = 2f; // time for one complete cycle

	// TODO: remove this from the inspector later
	[Range(0, 1)] float movementFactor; // 0 for not moved, 1 for fully moved

	Vector3 startPosition;

  // Start is called before the first frame update
  void Start() {
		startPosition = transform.position;
  }

  // Update is called once per frame
  void Update() {
		if (period <= Mathf.Epsilon) { // epsilon is the smallest possible float
			return; // prevent NaN error
		}
		const float tau = Mathf.PI * 2f; // roughly 2.68, 2 PI
		float cycles = Time.time / period; // Time.time is frame independent by default

		float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to 1

		movementFactor = rawSinWave / 2f + 0.5f;
		Vector3 offset = movementFactor * movementVector;
		transform.position = startPosition + offset;
  }
}
