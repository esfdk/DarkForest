using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {
	
	public void TakeControl()
	{
		// Enable the menu lights, text and textures.
		foreach(GameObject lightObject in GameObject.FindGameObjectsWithTag("MenuLight"))
		{
			lightObject.light.enabled = true;
		}
		
		foreach(GameObject menuThing in GameObject.FindGameObjectsWithTag("MenuObjects"))
		{
			if (menuThing.renderer != null) menuThing.renderer.enabled = true;
		}

		foreach(GameObject wispObject in GameObject.FindGameObjectsWithTag("Wisps_Alive"))
		{
			Destroy(wispObject);
		}
	}
}
