using UnityEngine;
using System.Collections;

public class MovementSoundPlay : MonoBehaviour {
	
	public AudioClip audioFile;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S)){
			audio.clip = audioFile;
			audio.Play();
		}
	}
}
