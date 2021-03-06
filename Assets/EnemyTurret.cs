﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour {

	bool alive = true;

	[SerializeField] ParticleSystem laser;
	[SerializeField] ParticleSystem stunEffect;

	AudioSource disabledSound;

	ScoreTracker scoreTracker;

	// Start is called before the first frame update
	void Start() {
		laser.Play();
		disabledSound = GetComponent<AudioSource>();
		scoreTracker = FindObjectOfType<ScoreTracker>();
		print(stunEffect);
	}

	// Update is called once per frame
	void Update() {
	}

	private void OnParticleCollision(GameObject collision) {
		print(collision.gameObject.tag);
		switch (collision.gameObject.tag) {
			case "Friendly Laser":
				if (alive) {
					scoreTracker.AddEnemyDestroyedScore();
					stunEffect.Play();
					laser.Stop();
					disabledSound.Play();
				}
				alive = false;
				break;
			default:
				break;
		}
	}
}
