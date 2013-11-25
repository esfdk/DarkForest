using UnityEngine;
using System.Collections;

public class Credits_Scrolling : MonoBehaviour {
	
	private string lowestCredits;
	
	private bool scrolling;
	
	private float speed = 2f;
	
	private Vector3 initialPosition;
	
	private Camera menuCamera;
	
	// Use this for initialization
	void Start () 
	{
		menuCamera = GameObject.Find("Menu Camera").camera;
		
		initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		
		lowestCredits = "Name_Jacob";
		
		scrolling = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		var camPos = menuCamera.transform;
		
		if (camPos.eulerAngles.y < 260 || camPos.eulerAngles.y > 340)
		{
			this.ResetPosition();
			scrolling = false;
		}
		else 
		{
			scrolling = true;
		}
		
		var lowestCreditsTransform = GameObject.Find(lowestCredits).transform;
		var creditsViewport = menuCamera.WorldToViewportPoint(lowestCreditsTransform.position);
		
		if (creditsViewport.y > 1.1f)
		{
			this.ResetPosition();	
		}
		
		if (scrolling) MoveCredits();	
	}
	
	private void MoveCredits()
	{
		this.transform.Translate(Vector3.up * Time.deltaTime * speed);
	}
	
	private void ResetPosition()
	{
		if (!this.transform.position.Equals(initialPosition))
		{
			var tempPosition = initialPosition;
			this.transform.position = tempPosition;
		}
	}
}
