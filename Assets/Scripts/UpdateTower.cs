using UnityEngine;
using System.Collections;

public class UpdateTower : MonoBehaviour {

	private float towerAngle = 0.0f;
	private float distanceToTower = 100f;
	private float change = 0.005f;
	
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
		var player = GameObject.Find("Player");
		var tPos = GameObject.Find("Tower").transform.position;
		var xDist = player.transform.position.x - tPos.x;
		var zDist = player.transform.position.z - tPos.z;
		var tDist = Mathf.Sqrt(Mathf.Pow(xDist, 2) + 0 + Mathf.Pow(zDist, 2));
		
		// If the player is close enough to the tower, respawn the player.
		if (tDist <= 10)
		{
			player.SendMessage("Respawn");
		}
	}
	
	/// <summary>
	/// Moves the tower.
	/// </summary>
	private void MoveTower()
	{
		var camera = GameObject.Find("Main Camera").camera;
		var towerTransform = GameObject.Find("Tower").transform;
		var player = GameObject.Find("Player").transform;
		
		var newTowerPosition = new Vector3(0, 0, 0);
		// Convert tower position to viewport points.
		var towerViewportPoint = camera.WorldToViewportPoint(new Vector3(towerTransform.position.x, player.position.y, towerTransform.position.z));
		
		// If tower is outside the camera on the right
		if (towerViewportPoint.x >= 1f)
		{ 
			// Calculate the world points that correspond to the edge of the camera
			var tempVector = camera.ViewportToWorldPoint(new Vector3(towerViewportPoint.x - change, towerViewportPoint.y, towerViewportPoint.z));
			newTowerPosition = tempVector;
			
			// Calculate a new angle for the tower
			var newAngle = ((Mathf.Rad2Deg * Mathf.Atan2(tempVector.x - player.position.x, tempVector.z - player.position.z)) + 360) % 360;
			towerAngle = newAngle;
		}
		// If tower is outside the camera on the left
		else if (towerViewportPoint.x <= 0f)
		{
			// Calculate the world points that correspond to the edge of the camera
			var tempVector = camera.ViewportToWorldPoint(new Vector3(towerViewportPoint.x + change, towerViewportPoint.y, towerViewportPoint.z));
			newTowerPosition = tempVector;
			
			// Calculate a new angle for the tower
			var newAngle = ((Mathf.Rad2Deg * Mathf.Atan2(tempVector.x - player.position.x, tempVector.z - player.position.z)) + 360) % 360;
			towerAngle = newAngle;
		}
		else 
		{
			// Fix the position of the tower based on its angle.
			var newX = player.position.x + distanceToTower * Mathf.Sin(towerAngle * Mathf.Deg2Rad);
			var newZ = player.position.z + distanceToTower * Mathf.Cos(towerAngle * Mathf.Deg2Rad);
			
			newTowerPosition.x = newX;
			newTowerPosition.z = newZ;
		}
		
		var test = Terrain.activeTerrain.SampleHeight(newTowerPosition) - 5;
		newTowerPosition.y = test;
		
		towerTransform.position = newTowerPosition;
	}
	
	/// <summary>
	/// Changes the distance to the tower.
	/// </summary>
	private void ChangeDistanceToTower()
	{
		// Get player distance from the middle of the map.
		var pPos = GameObject.Find("Player").transform.position;
		var xDist = pPos.x - 625;
		var zDist = pPos.z - 625;
		var tDist = Mathf.Sqrt(Mathf.Pow(xDist, 2) + 0 + Mathf.Pow(zDist, 2));
		
		var newDistance = 625 - tDist;
		
		// Change distanceToTower if the player is approaching the edge of the map.
		if (newDistance < 100)
		{
			if (newDistance < 0) 	distanceToTower = 0;
			else 					distanceToTower = newDistance;
		}
		else
		{
			distanceToTower = 100;
		}
	}
}
