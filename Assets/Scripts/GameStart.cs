using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		Screen.showCursor = true;
		
		GameObject.Find("Main Camera").camera.enabled = false;
		GameObject.Find("Menu Camera").camera.enabled = true;
	}
}
