using UnityEngine;
using System.Collections;

public class Credits_Scrolling : MonoBehaviour {
	
	private string lowestCredits;
	private bool scrolling;
	private float speed = 2f;
	private Vector3 initialPosition;
	private Camera menuCamera;
	private Transform lowestCreditsTransform;
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{
		menuCamera = GameObject.Find("Menu Camera").camera;
		lowestCreditsTransform = GameObject.Find("Name_Jacob").transform;
		
		initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		scrolling = false;
	}
	
	/// <summary>
	/// Update this instance (called once per frame).
	/// </summary>
	void Update () 
	{
		var camPos = menuCamera.transform;

		// Check if the camera is pointing towards the credits screen.
		if (camPos.eulerAngles.y < 260 || camPos.eulerAngles.y > 340)
		{
			this.ResetPosition();
			scrolling = false;
		}
		else 
		{
			scrolling = true;
		}

		// Loop the credits if they have exited the top of the screen.
		var creditsViewport = menuCamera.WorldToViewportPoint(lowestCreditsTransform.position);
		
		if (creditsViewport.y > 1.1f)
		{
			this.ResetPosition();	
		}

		// Move the credits.
		if (scrolling) MoveCredits();	
	}

	/// <summary>
	/// Moves the credits.
	/// </summary>
	private void MoveCredits()
	{
		this.transform.Translate(Vector3.up * Time.deltaTime * speed);
	}

	/// <summary>
	/// Resets the position of the credits.
	/// </summary>
	private void ResetPosition()
	{
		if (!this.transform.position.Equals(initialPosition))
		{
			var tempPosition = initialPosition;
			this.transform.position = tempPosition;
		}
	}
}
