using UnityEngine;
using System.Collections;

public class UpdateTower : MonoBehaviour {

	private float towerAngle = 0.0f;
	private float distanceToTower = 100f;
	private float change = 0.002f;
	private float distanceRatio = 1.6f;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{		
		MoveTower();
	}
	
	private void MoveTower()
	{
		var camera = GameObject.Find("Main Camera").camera;
		var towerTransform = GameObject.Find("Tower").transform;
		var player = GameObject.Find("Player").transform;
		
		var newTowerPosition = new Vector3(0, 75, 0);
		var towerViewportPoint = camera.WorldToViewportPoint(new Vector3(towerTransform.position.x, player.position.y, towerTransform.position.z));
		
		if (towerViewportPoint.x >= 1f)
		{ 
			var tempVector = camera.ViewportToWorldPoint(new Vector3(towerViewportPoint.x - change, towerViewportPoint.y, towerViewportPoint.z));
			tempVector.y = newTowerPosition.y;
			newTowerPosition = tempVector;
			
			var newAngle = ((Mathf.Rad2Deg * Mathf.Atan2(tempVector.x - player.position.x, tempVector.z - player.position.z)) + 360) % 360;
			towerAngle = newAngle;
		}
		else if (towerViewportPoint.x <= 0f)
		{
			var tempVector = camera.ViewportToWorldPoint(new Vector3(towerViewportPoint.x + change, towerViewportPoint.y, towerViewportPoint.z));
			tempVector.y = newTowerPosition.y;
			newTowerPosition = tempVector;
			
			var newAngle = ((Mathf.Rad2Deg * Mathf.Atan2(tempVector.x - player.position.x, tempVector.z - player.position.z)) + 360) % 360;
			towerAngle = newAngle;
		}
		else 
		{
			var newX = player.position.x + distanceToTower * Mathf.Sin(towerAngle * Mathf.Deg2Rad);
			var newZ = player.position.z + distanceToTower * Mathf.Cos(towerAngle * Mathf.Deg2Rad);
			
			newTowerPosition.x = newX;
			newTowerPosition.z = newZ;
		}
		
		towerTransform.position = newTowerPosition;
	}
}
