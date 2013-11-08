using UnityEngine;
using System.Collections;

public class WispSpawner : MonoBehaviour 
{
	private string GateEastTag = "GateEast";
	private Vector3 GateEastLocation;
	
	private string GateWestTag = "GateWest";
	private Vector3 GateWestLocation;
	
	private string ChurchTag = "Church";
	private Vector3 ChurchLocation;
	
	private string HousesEastTag = "HousesEast";
	private Vector3 HouseEastLocation;
	
	private string HousesWestTag = "HousesWest";
	private Vector3 HouseWestLocation;
	
	private string GraveyardTag = "Graveyard";
	private Vector3 GraveyardLocation;
	
	private string HillTag = "Hill";
	private Vector3 HillLocation;
	
	private string ClearingNorthTag = "ClearingNorth";
	private Vector3 ClearingNorthLocation;
	
	private string ClearingSouthTag = "ClearingSouth";
	private Vector3 ClearingSouthLocation;
	
	private string PillarNETag = "PillarNE";
	private Vector3 PillarNELocation;
	
	private string PillarNWTag = "PillarNW";
	private Vector3 PillarNWLocation;
	
	private string PillarSETag = "PillarSE";
	private Vector3 PillarSELocation;
	
	private string PillarSWTag = "PillarSW";
	private Vector3 PillarSWLocation;
	
	private int heightFromTerrain = 3;
	
	public GameObject wisp;
	
	// Use this for initialization
	void Start () 
	{
		GateEastLocation = CreateLocation(745, 710);
		GateWestLocation = CreateLocation(490, 660);
		
		ChurchLocation = CreateLocation(450, 550);
		HouseEastLocation = CreateLocation(545, 590);
		HouseWestLocation = CreateLocation(670, 720);
		
		GraveyardLocation = CreateLocation(490, 600);
		HillLocation = CreateLocation(770, 590);
		
		ClearingNorthLocation = CreateLocation(560, 820);
		ClearingSouthLocation = CreateLocation(625, 445);
		
		PillarNELocation = CreateLocation(730, 770);
		PillarNWLocation = CreateLocation(510, 770);
		PillarSELocation = CreateLocation(510, 480);
		PillarSWLocation = CreateLocation(730, 480);
	}
	
	/// <summary>
	/// Creates a location with the terrain's height based on the x and z coordinates.
	/// </summary>
	/// <returns> The location. </returns>
	/// <param name='x'> The x-coordinate. </param>
	/// <param name='z'> The z-coordinate. </param>
	private Vector3 CreateLocation(int x, int z)
	{
		var tempVector = new Vector3(x, 0, z);
		var terrainY = Terrain.activeTerrain.SampleHeight(tempVector) + heightFromTerrain;
		tempVector.y = terrainY;
		
		return tempVector;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) 
	{	
		var w = (GameObject) Instantiate(wisp, other.gameObject.transform.position, other.gameObject.transform.rotation);
		
		switch (other.gameObject.tag)
		{
			case GateEastTag:
				w.GetComponent<RandomWispMovement>().end = GateEastLocation;
				break;
			
			case GateWestTag:
				w.GetComponent<RandomWispMovement>().end = GateWestLocation;
				break;
			
			case ChurchTag:
				w.GetComponent<RandomWispMovement>().end = ChurchLocation;
				break;
			
			case HousesEastTag:
				w.GetComponent<RandomWispMovement>().end = HouseEastLocation;
				break;
			
			case HousesWestTag:
				w.GetComponent<RandomWispMovement>().end = HouseWestLocation;
				break;
			
			case GraveyardTag:
				w.GetComponent<RandomWispMovement>().end = GraveyardLocation;
				break;
			
			case HillTag:
				w.GetComponent<RandomWispMovement>().end = HillLocation;
				break;
			
			case ClearingNorthTag:
				w.GetComponent<RandomWispMovement>().end = ClearingNorthLocation;
				break;
			
			case ClearingSouthTag:
				w.GetComponent<RandomWispMovement>().end = ClearingSouthLocation;
				break;
			
			case PillarNETag:
				w.GetComponent<RandomWispMovement>().end = PillarNELocation;
				break;
			
			case PillarNWTag:
				w.GetComponent<RandomWispMovement>().end = PillarNWLocation;
				break;
			
			case PillarSETag:
				w.GetComponent<RandomWispMovement>().end = PillarSELocation;
				break;
			
			case PillarSWTag:
				w.GetComponent<RandomWispMovement>().end = PillarSWLocation;
				break;
		}
	}
}
