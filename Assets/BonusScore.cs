using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScore : MonoBehaviour {

	BoxCollider trigger;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

	private void OnCollisionEnter(Collision collision) {
		print(collision.gameObject);
		if (collision.gameObject.tag == "Player") {
			print("violated!");
			this.gameObject.SetActive(false);
		}
	}

	private void OnTriggerEnter(Collider other) {
		print("of course");
	}
}
