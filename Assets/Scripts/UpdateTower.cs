using UnityEngine;
using System.Collections;

public class UpdateTower : MonoBehaviour {
	
	private GameObject player;
	
	private Camera gameCamera;
	
	private bool locked = false;
	
	private float distanceToMid = 610f;
	private float towerAngle = 0.0f;
	private float distanceToTower = 100f;
	private float change = 0.005f;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		player = GameObject.Find("Player");
		gameCamera = GameObject.Find("Main Camera").camera;
	}

	/// <summary>
	/// Update this instance (called once per frame).
	/// </summary>
	void Update () 
	{
		ChangeDistanceToTower();
		MoveTower();
	}
	
	/// <summary>
	/// Moves the tower.
	/// </summary>
	private void MoveTower()
	{
		if (locked) return;

		var towerTransform = this.transform;
		
		var newTowerPosition = towerTransform.position;
		// Convert tower position to viewport points.
		var towerViewportPoint = gameCamera.WorldToViewportPoint(new Vector3(towerTransform.position.x, player.transform.position.y, towerTransform.position.z));

		var lookAngle = gameCamera.transform.eulerAngles.x > 180 ? -360 : 0;
		var angleChange = 0.15f * (Mathf.Abs(lookAngle + gameCamera.transform.eulerAngles.x) / 5f);

		// If tower is outside the camera on the right
		if (towerViewportPoint.x >= 1.2f + angleChange)
		{ 
			// Calculate the world points that correspond to the edge of the camera
			var tempVector = gameCamera.ViewportToWorldPoint(new Vector3(towerViewportPoint.x - change, towerViewportPoint.y, towerViewportPoint.z));
			newTowerPosition = tempVector;
			
			// Calculate a new angle for the tower
			var newAngle = ((Mathf.Rad2Deg * Mathf.Atan2(tempVector.x - player.transform.position.x, tempVector.z - player.transform.position.z)) + 360) % 360;
			towerAngle = newAngle;
		}
		// If tower is outside the camera on the left
		else if ((towerViewportPoint.x <= -0.2f - angleChange || towerViewportPoint.z < 0f))
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
		
		var test = Terrain.activeTerrains[1].SampleHeight(newTowerPosition) - 1f;
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

			if (this.transform.parent != null)
			{
				var newPos = GameObject.Find("Ground").transform.TransformPoint(this.transform.position);
				this.transform.parent = null;
				this.transform.position = newPos;
			}
		}
		else
		{
			distanceToTower = 100;
			locked = false;

			if (this.transform.parent == null) this.transform.parent = player.transform;
		}
	}
}
