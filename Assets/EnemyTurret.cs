using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour {
	bool alive = true;

	[SerializeField] ParticleSystem laser;
	[SerializeField] ParticleSystem stunEffect;

	// Start is called before the first frame update
	void Start() {
		laser.Play();
	}

	// Update is called once per frame
	void Update() {
		if (!alive) {
			laser.Stop();
		}
	}

	private void OnParticleCollision(GameObject collision) {
		print(collision.gameObject.tag);
		switch (collision.gameObject.tag) {
			case "Friendly Laser":
				alive = false;
				stunEffect.Play();
				break;
			default:
				break;
		}
	}
}
