using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreenRocket : MonoBehaviour {


	AudioSource audioSource;

	[SerializeField] AudioClip thrustAudio;
	[SerializeField] AudioClip deathAudio;

	[SerializeField] ParticleSystem thrustParticles;
	[SerializeField] ParticleSystem explosionParticles;

	Vector3 rocketStartPosition;

	bool startingGame  = false;

	Rigidbody rocketRigidbody;

	int frameCount = 0;

	// Start is called before the first frame update
	void Start() {
		thrustParticles.Play();
		audioSource = GetComponent<AudioSource>();
		rocketRigidbody = GetComponent<Rigidbody>();

	}

  // Update is called once per frame
  void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			audioSource.PlayOneShot(thrustAudio);

			startingGame = true;

			Invoke("StartGame", 3.5f);
		}

		if (startingGame) {
			frameCount++;

			Vector3 currentPosition = gameObject.transform.position;
			Vector3 updatedPosition = new Vector3(currentPosition.x, currentPosition.y + 1.5f * Time.deltaTime * frameCount, currentPosition.z);

			this.gameObject.transform.position = updatedPosition;
		}
	}

	private void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag) {
			case "Start":
				print("on the launch pad");
				break;
			default:
				startingGame = false;
				rocketRigidbody.useGravity = true;
				HandleDeath();
				break;
		}
	}

	private void HandleDeath() {
		audioSource.Stop();
		audioSource.PlayOneShot(deathAudio);

		thrustParticles.Stop();
		print(explosionParticles);
		explosionParticles.Play();
		print(explosionParticles.isPlaying);
	}

	private void StartGame() {
		SceneManager.LoadScene(1);
	}
}
