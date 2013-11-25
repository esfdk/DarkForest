﻿using UnityEngine;
using System.Collections;

public class UpdateTower : MonoBehaviour {
	
	private GameObject player;
	private GameObject colliders;
	
	private Camera gameCamera;
	
	private bool locked = false;
	
	private float distanceToMid = 625f;
	private float towerAngle = 0.0f;
	private float distanceToTower = 100f;
	private float change = 0.005f;
	
	void Start()
	{
		player = GameObject.Find("Player");
		gameCamera = GameObject.Find("Main Camera").camera;
		
		
		colliders = GameObject.FindGameObjectWithTag("Testshit");
		colliders.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		ChangeDistanceToTower();
		MoveTower();
		CheckForPlayerCollision();
	}
	
	/// <summary>
	/// Checks if the player is colliding with the tower.
	/// Respawns the player if true.
	/// </summary>
	private void CheckForPlayerCollision()
	{
		// Determine the distance to the player.
		var tPos = this.transform.position;
		var xDist = player.transform.position.x - tPos.x;
		var zDist = player.transform.position.z - tPos.z;
		var tDist = Mathf.Sqrt(Mathf.Pow(xDist, 2f) + 0f + Mathf.Pow(zDist, 2f));
		
		// If the player is close enough to the tower, respawn the player.
		if (tDist <= 10f)
		{
			player.SendMessage("Respawn");
		}
	}
	
	/// <summary>
	/// Moves the tower.
	/// </summary>
	private void MoveTower()
	{
		var towerTransform = this.transform;
		
		var newTowerPosition = new Vector3(0f, 0f, 0f);
		// Convert tower position to viewport points.
		var towerViewportPoint = gameCamera.WorldToViewportPoint(new Vector3(towerTransform.position.x, player.transform.position.y, towerTransform.position.z));

		// If tower is outside the camera on the right
		if (towerViewportPoint.x >= 1f && locked == false)
		{ 
			// Calculate the world points that correspond to the edge of the camera
			var tempVector = gameCamera.ViewportToWorldPoint(new Vector3(towerViewportPoint.x - change, towerViewportPoint.y, towerViewportPoint.z));
			newTowerPosition = tempVector;
			
			// Calculate a new angle for the tower
			var newAngle = ((Mathf.Rad2Deg * Mathf.Atan2(tempVector.x - player.transform.position.x, tempVector.z - player.transform.position.z)) + 360) % 360;
			towerAngle = newAngle;
		}
		// If tower is outside the camera on the left
		else if ((towerViewportPoint.x <= 0f || towerViewportPoint.z < 0f)
					&& locked == false)
		{
			// Calculate the world points that correspond to the edge of the camera
			var tempVector = gameCamera.ViewportToWorldPoint(new Vector3(towerViewportPoint.x + change, towerViewportPoint.y, Mathf.Abs(towerViewportPoint.z)));
			newTowerPosition = tempVector;
			
			// Calculate a new angle for the tower
			var newAngle = ((Mathf.Rad2Deg * Mathf.Atan2(tempVector.x - player.transform.position.x, tempVector.z - player.transform.position.z)) + 360) % 360;
			towerAngle = newAngle;
		}
		else 
		{
			// Fix the position of the tower based on its angle.
			var newX = player.transform.position.x + distanceToTower * Mathf.Sin(towerAngle * Mathf.Deg2Rad);
			var newZ = player.transform.position.z + distanceToTower * Mathf.Cos(towerAngle * Mathf.Deg2Rad);
			
			newTowerPosition.x = newX;
			newTowerPosition.z = newZ;
		}
		
		var test = Terrain.activeTerrain.SampleHeight(newTowerPosition) - 5f;
		newTowerPosition.y = test;
		
		towerTransform.position = newTowerPosition;
	}
	
	/// <summary>
	/// Changes the distance to the tower.
	/// </summary>
	private void ChangeDistanceToTower()
	{
		// Get player distance from the middle of the map.
		var pPos = player.transform.position;
		var xDist = pPos.x - distanceToMid;
		var zDist = pPos.z - distanceToMid;
		var tDist = Mathf.Sqrt(Mathf.Pow(xDist, 2f) + 0f + Mathf.Pow(zDist, 2f));
		
		var newDistance = distanceToMid - tDist;
		
		// Change distanceToTower if the player is approaching the edge of the map.
		if (newDistance < 100)
		{
			if (newDistance < 0)
			{
				distanceToTower = 0;
			}
			else
			{
				distanceToTower = newDistance;
			}
			
			locked = true;

			this.transform.parent = null;
			colliders.SetActive(true);
			var fuckyouunity = this.GetComponent<UpdateTower>();
			fuckyouunity.enabled = false;
		}
		else
		{
			distanceToTower = 100;
			locked = false;
			
			colliders.SetActive(false);
			this.transform.parent = player.transform;
		}
	}
}
