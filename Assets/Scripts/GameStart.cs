using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Screen.showCursor = true; 
		
		GameObject.Find("Main Camera").camera.enabled = false;
		GameObject.Find("Menu Camera").camera.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
