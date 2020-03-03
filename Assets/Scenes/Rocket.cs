using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;

	// '[SerializeField]' allows variables to be changed from the Unity Inspector
	[SerializeField] float progradeThrustMulitplier = 2000f;
	[SerializeField] float rotationMultiplier = 400f;

	[SerializeField] AudioClip thrustAudio;
	[SerializeField] AudioClip deathAudio;
	[SerializeField] AudioClip transcendAudio;

	[SerializeField] ParticleSystem thrustParticles;
	[SerializeField] ParticleSystem explosionParticles;
	[SerializeField] ParticleSystem successParticles;

	int currentLevel = 0;

	enum State { Alive, Dying, Transcending };
	State state = State.Alive;

	// Start is called before the first frame update
	void Start() {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update() {
		HandleInput();
	}

	private void LoadFirstLevel() {
		currentLevel = 0;

		SceneManager.LoadScene(currentLevel);
	}

	private void LoadNextLevel() {
		currentLevel++;

		SceneManager.LoadScene(currentLevel);
	}

	private void OnCollisionEnter(Collision collision) {
		if (state == State.Alive) {
			switch(collision.gameObject.tag) {
				case "Friendly":
					print("on the launch pad");
					break;
				case "Finish":
					HandleLevelComplete();
					break;
				case "Fuel":
					print("Gassed up homie");
					break;
				default:
					HandleDeath();
					break;
			}
		}
	}

	private void HandleLevelComplete() {
		audioSource.Stop();
		audioSource.PlayOneShot(transcendAudio);

		thrustParticles.Stop();
		successParticles.Play();

		state = State.Transcending;

		Invoke("LoadNextLevel", 2.1f); // todo: paramatarize time
	}

	private void HandleDeath() {
		audioSource.Stop();
		audioSource.PlayOneShot(deathAudio);

		thrustParticles.Stop();
		explosionParticles.Play();

		state = State.Dying;

		Invoke("LoadFirstLevel", 2f);
	}

	private void HandleInput() {

		if (state == State.Alive) { // disallow controls while transitioning
			rigidBody.freezeRotation = true; // disallows external control, prevents physics induced spin

			// can always thrust, even while rotating
			RespondToThrustInput();

			//// stop playing when space is released
			//if (Input.GetKeyUp(KeyCode.Space)) {
			//	audioSource.Stop();
			//}

			RespondToRotateInput();
		}

		rigidBody.freezeRotation = false; // once player control is released, reallow physics induced spin
	}

	private void RespondToThrustInput() {
		if (Input.GetKey(KeyCode.Space)) {

			// make rocket noises bbrrgrgrrrrhhrhrhr
			if (!audioSource.isPlaying) { // prevents starting the clip on every frame;
				audioSource.PlayOneShot(thrustAudio);
				thrustParticles.Play();
			}
			
			// apply thrust
			rigidBody.AddRelativeForce(Vector3.up * progradeThrustMulitplier * Time.deltaTime); // multiplying by Time.deltaTime ensures frame independence
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			audioSource.Stop();
			thrustParticles.Stop();
		}
	}

	private void RespondToRotateInput() {
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Vector3.forward * rotationMultiplier * Time.deltaTime); // left handed coordinate system in the z-axis. this means +ve values go counter-clockwise
		} else if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(Vector3.back * rotationMultiplier * Time.deltaTime);
		}
	}
}