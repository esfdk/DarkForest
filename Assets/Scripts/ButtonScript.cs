using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {
	
	private Vector3 StartRotation = new Vector3(0, 0, 0);
	private Vector3 ControlsRotation = new Vector3(0, 90, 0);
	private Vector3 CreditsRotation = new Vector3(0, 270, 0);
	
	private float speed = 0.5f * 15;
	
	void OnMouseDown()
	{
		
		switch (this.name)
		{
			case "Start_PlayBT":
				Debug.Log ("Start_PlayBT clicked");
				break;
			
			case "Start_ControlsBT":
				iTween.RotateTo(Camera.main.gameObject, ControlsRotation, speed);
				Debug.Log ("Start_ControlsBT clicked");
				break;
			
			case "Start_CreditsBT":
				iTween.RotateTo(Camera.main.gameObject, CreditsRotation, speed);
				Debug.Log ("Start_CreditsBT clicked");
				break;
			
			case "Control_BackBT":
				Debug.Log ("Control_BackBT clicked");
				iTween.RotateTo(Camera.main.gameObject, StartRotation, speed);
				break;
			
			case "Credits_BackBT":
				Debug.Log ("Credits_BackBT clicked");
				iTween.RotateTo(Camera.main.gameObject, StartRotation, speed);
				break;
		}
		
	        // load the game
	        //Application.LoadLevel("game");
	}
}
