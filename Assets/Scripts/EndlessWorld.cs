using UnityEngine;
using System.Collections;

public class EndlessWorld : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		var p = transform.position;
		switch(other.gameObject.tag)
		{
			case "WestTerrain": 
				p = new Vector3(p.x + 1250, p.y, p.z);
				transform.position = p;
				Debug.Log("it worked - W");
				break;
			case "EastTerrain": 
				p = new Vector3(p.x - 1250, p.y, p.z);
				transform.position = p;
				Debug.Log("it worked - E");
				break;
			case "NorthTerrain": 
				p = new Vector3(p.x, p.y, p.z - 1250);
				transform.position = p;
				Debug.Log("it worked - N");
				break;
			case "SouthTerrain": 
				p = new Vector3(p.x, p.y, p.z + 1250);
				transform.position = p;
				Debug.Log("it worked - S");
				break;
		}
	}
}
