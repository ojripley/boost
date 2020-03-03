using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	Rigidbody rigidBody;
	AudioSource audioSource;

	// '[SerializeField]' allows variables to be changed from the Unity Inspector
	[SerializeField] float progradeThrustMulitplier = 2000f;
	[SerializeField] float rotationMultiplier = 400f;

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
		handleInput();

		handleAudioOnState();
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
		switch(collision.gameObject.tag) {
			case "Friendly":
				print("on the launch pad");
				break;
			case "Finish":
				print("You beat the level!");
				state = State.Transcending;
				Invoke("LoadNextLevel", 1f); // todo: paramatarize time
				
				break;
			case "Fuel":
				print("Gassed up homie");
				break;
			default:
				print("dead");
				state = State.Dying;
				Invoke("LoadFirstLevel", 1f);
				break;
		}
	}

	private void handleInput() {
		rigidBody.freezeRotation = true; // disallows external control, prevents physics induced spin

		if (state != State.Dying && state != State.Transcending) { // disallow controls while transitioning

			// can always thrust, even while rotating
			if (Input.GetKey(KeyCode.Space)) {


				rigidBody.AddRelativeForce(Vector3.up * progradeThrustMulitplier);
				handleAudio();
			}

			// stop playing when space is released
			if (Input.GetKeyUp(KeyCode.Space)) {
				audioSource.Stop();
			}

			if (Input.GetKey(KeyCode.A)) {
				transform.Rotate(Vector3.forward * rotationMultiplier * Time.deltaTime); // left handed coordinate system in the z-axis. this means +ve values go counter-clockwise
			} else if (Input.GetKey(KeyCode.D)) {
				transform.Rotate(Vector3.back * rotationMultiplier * Time.deltaTime);
			}
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

	private void handleAudioOnState() {
		if ((state == State.Dying || state == State.Transcending) && audioSource.isPlaying) {
			audioSource.Stop();
		}
	}
}