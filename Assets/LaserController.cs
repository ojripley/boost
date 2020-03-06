using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LaserController : MonoBehaviour {

	[SerializeField] int RandomUpperBoundOn = 50;
	[SerializeField] int RandomUpperBoundOff = 2;


	// Start is called before the first frame update
	void Start() {
	}

	// Update is called once per frame
	void Update() {

		ParticleSystem[] lasers = GameObject.FindObjectsOfType<ParticleSystem>();

		System.Random random = new System.Random();

		int chanceOn = random.Next(0, RandomUpperBoundOn);
		int chanceOff = random.Next(0, RandomUpperBoundOff);


		if (chanceOn == 1) {
			int chanceLaserOn = random.Next(0, lasers.Length);

			if (lasers[chanceLaserOn].name[0] == 'T') {
				lasers[chanceLaserOn].Play(true);
			}
		}


		if (chanceOff == 0) {

			int chanceLaserOff = random.Next(0, lasers.Length);

			if (lasers[chanceLaserOff].name[0] == 'T') {
				lasers[chanceLaserOff].Stop(true);
			}
		}
	}
}