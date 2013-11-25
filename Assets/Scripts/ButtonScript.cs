using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	
	private Vector3 StartRotation = new Vector3(0, 0, 0);
	private Vector3 ControlsRotation = new Vector3(0, 90, 0);
	private Vector3 CreditsRotation = new Vector3(0, 270, 0);
	
	private float speed = 0.5f * 15;
	
	private Hashtable rotateParams;
	
	void OnMouseDown()
	{
		switch (this.name)
		{
			case "Start_PlayBT":
				this.StartGame();
				break;
			
			case "Start_ControlsBT":
				RotateCamera(ControlsRotation);
				break;
			
			case "Start_CreditsBT":
				RotateCamera(CreditsRotation);
				break;
			
			case "Start_QuitBT":
				Application.Quit();
				break;
			
			case "Control_BackBT":
				RotateCamera(StartRotation);
				break;
			
			case "Credits_BackBT":
				RotateCamera(StartRotation);
				break;
		}
	}
	
	private void StartGame()
	{
		Screen.showCursor = false;
		GameObject.Find("Menu Camera").camera.enabled = false;
		
		var menuLights = GameObject.FindGameObjectsWithTag("MenuLight");
		
		foreach(GameObject lightObject in menuLights)
		{
			lightObject.light.enabled = false;
		}
		
		foreach(GameObject menuThing in GameObject.FindGameObjectsWithTag("MenuObjects"))
		{
			if (menuThing.renderer != null) menuThing.renderer.enabled = false;
		}
		
		GameObject.Find("Main Camera").camera.enabled = true;
		GameObject.Find("Player").gameObject.SendMessage("Respawn");
		
		var player = GameObject.Find("Player").transform;
		var start = new Vector3(player.position.x, player.position.y, player.position.z + 2);
		var w = (GameObject) Instantiate(Resources.Load("Wisp/Wisp"), start, player.rotation);
		var wisp = w.GetComponent<RandomWispMovement>();
		var rosePosition = GameObject.Find("Rose").transform.position;
		wisp.end = new Vector3(rosePosition.x, 2, rosePosition.z);
		
		AudioSource.PlayClipAtPoint((AudioClip) Resources.Load("Wisp/Wisp spawn"), start);
	}
	
	/// <summary>
	/// Rotates the camera to the given position.
	/// </summary>
	/// <param name='newPosition'> The new position. </param>
	private void RotateCamera(Vector3 newPosition)
	{
		rotateParams = new Hashtable();
		
		rotateParams.Add(iT.RotateTo.rotation, newPosition);
		rotateParams.Add(iT.RotateTo.speed, speed);
		
		iTween.RotateTo(Camera.main.gameObject, rotateParams);
	}
}
