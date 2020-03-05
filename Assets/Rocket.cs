using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	// debugging
	public bool collisionsEnabled = true;

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

	[SerializeField] ParticleSystem laser;

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

		if (Debug.isDebugBuild) {
			HandleDebugInput();
		}
	}

	private void LoadFirstLevel() {
		SceneManager.LoadScene(0);
	}

	private void LoadNextLevel() {
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		currentSceneIndex++;

		if (currentSceneIndex == SceneManager.sceneCountInBuildSettings) {
			LoadFirstLevel();
		} else {
			SceneManager.LoadScene(currentSceneIndex);
		}
	}

	private void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void OnCollisionEnter(Collision collision) {
		if (state == State.Alive && collisionsEnabled) {
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

		Invoke("LoadNextLevel", 2.1f);
	}

	private void HandleDeath() {
		audioSource.Stop();
		audioSource.PlayOneShot(deathAudio);

		laser.Stop();
		thrustParticles.Stop();
		explosionParticles.Play();

		state = State.Dying;

		Invoke("ReloadLevel", 2f);
	}

	private void HandleInput() {

		if (state == State.Alive) { // disallow controls while transitioning
			rigidBody.freezeRotation = true; // disallows external control, prevents physics induced spin

			// can always thrust, even while rotating
			RespondToThrustInput();

			RespondToRotateInput();

			RespondToUtilities();
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

	private void RespondToUtilities() {
		if (Input.GetMouseButtonDown(0)) {
			laser.Play();
		}

		if (Input.GetMouseButtonUp(0)) {
			Invoke("StopLaser", 0.1f);
		}
	}

	private void StopLaser() {
		laser.Stop();
	}

	private void HandleDebugInput() {
		if (Input.GetKeyDown(KeyCode.L)) {
			LoadNextLevel();
		} else if (Input.GetKeyDown(KeyCode.C)) {
			collisionsEnabled = !collisionsEnabled;
		}
	}
}