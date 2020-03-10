using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	// debugging
	public bool collisionsEnabled = true;

	HUD hud;
	UI ui;
	ScoreTracker scoreTracker;

	Rigidbody rigidBody;
	AudioSource audioSource;

	// '[SerializeField]' allows variables to be changed from the Unity Inspector
	[SerializeField] float progradeThrustMulitplier = 2000f;
	[SerializeField] float rotationMultiplier = 300f;

	[SerializeField] AudioClip thrustAudio;
	[SerializeField] AudioClip deathAudio;
	[SerializeField] AudioClip transcendAudio;

	[SerializeField] ParticleSystem thrustParticles;
	[SerializeField] ParticleSystem explosionParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem bonusScoreParticles;

	[SerializeField] ParticleSystem laser;

	public enum State { Alive, Dying, Transcending };
	public State state = State.Alive;
	bool hasStarted = false;
	int shieldLayers = 0;


	// Start is called before the first frame update
	void Start() {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		hud = FindObjectOfType<HUD>();
		ui = FindObjectOfType<UI>();
		scoreTracker = FindObjectOfType<ScoreTracker>();
		ui.SetLevelCompleteMenuDisplay(false);
	}

	// Update is called once per frame
	void Update() {
		HandleInput();

		if (Debug.isDebugBuild) {
			HandleDebugInput();
		}
	}

	private void ReloadLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void OnTriggerEnter(Collider collider) {
		if (state == State.Alive) {
			switch (collider.gameObject.tag) {
				case "Bonus Score":
					scoreTracker.AddBonusScore();
					GameObject bonus = GameObject.FindGameObjectWithTag("Bonus Score Pickup");
					bonusScoreParticles.Play();
					bonus.SetActive(false);
					break;
				case "Enemy Laser":
					break;
				case "Fuel":
					break;
				default:
					break;
			}
		}
	}

	private void OnParticleCollision(GameObject collision) {
		if (state == State.Alive) {
			switch(collision.gameObject.tag) {
				case "Enemy Laser":
					HandleDeath();
					break;
				default:
					break;
			}
		}
	}

	private void OnCollisionEnter(Collision collision) {
		//print (collision.gameObject.name);
		if (state == State.Alive && collisionsEnabled) {
			switch(collision.gameObject.tag) {
				case "Start":
					//print("on the launch pad");
					break;
				case "Finish":
					hud.GetFinalTime();
					HandleLevelComplete();
					break;
				case "Fuel":
					//print("Gassed up homie");
					break;
				default:
					HandleDeath();
					break;
			}
		}
	}

	private void OnCollisionExit(Collision collision) {
		if (state == State.Alive && collisionsEnabled) {
			switch (collision.gameObject.tag) {
				default:
					if (shieldLayers > 0) {
						shieldLayers--;
						print(shieldLayers);
					}
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

		scoreTracker.AddLandingScore();

		ui.SetFinalTimeAndScore(hud.GetFinalTime(), scoreTracker.GetBaseScore());
		ui.SetLevelCompleteMenuDisplay(true);

		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
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

		if (Input.anyKey && !hasStarted) {
			hud.BeginCounting();
			hasStarted = true;
		}

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
			if (laser.isPlaying) {
				Invoke("StopLaser", 0.1f);
			}
		}
	}

	private void StopLaser() {
		if (!Input.GetMouseButton(0)) {
			laser.Stop();
		}
	}

	private void HandleDebugInput() {
		if (Input.GetKeyDown(KeyCode.L)) {
			//LoadNextLevel();
		} else if (Input.GetKeyDown(KeyCode.C)) {
			collisionsEnabled = !collisionsEnabled;
		}
	}
}