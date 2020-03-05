using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreenRocket : MonoBehaviour {

	[SerializeField] ParticleSystem thrustParticles;

	// Start is called before the first frame update
	void Start() {
		thrustParticles.Play();
  }

  // Update is called once per frame
  void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			SceneManager.LoadScene(1);
		}
	}
}
