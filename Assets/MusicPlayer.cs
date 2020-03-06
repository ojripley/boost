using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {
  
	// Start is called before the first frame update
  void Start() {
		int numberOfMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;

		// singleton pattern. Deletes self if another player is found
		if (numberOfMusicPlayers > 1) {
			Destroy(this.gameObject);
		} else {
			DontDestroyOnLoad(this.gameObject);
		}
	}
}
