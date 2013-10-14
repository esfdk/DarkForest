using UnityEngine;
using System.Collections;

public class MovementSoundMute : MonoBehaviour {
	
	public AudioClip audioFile;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S)){
			if(!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey (KeyCode.S))){
				audio.clip = audioFile;
				audio.Stop();
			}
		}
	}
}
