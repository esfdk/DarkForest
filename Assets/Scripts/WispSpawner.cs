using UnityEngine;
using System.Collections;

public class WispSpawner : MonoBehaviour 
{	
	private WispData GateEast = new WispData("GateEast");
	private WispData GateWest = new WispData("GateWest");
	private WispData Church = new WispData("Church");
	private WispData HousesEast = new WispData("HousesEast");
	private WispData HousesWest = new WispData("HousesWest");
	private WispData Graveyard = new WispData("Graveyard");
	private WispData Hill = new WispData("Hill");
	private WispData ClearingNorth = new WispData("ClearingNorth");
	private WispData ClearingSouth = new WispData("ClearingSouth");
	private WispData PillarNE = new WispData("PillarNE");
	private WispData PillarNW = new WispData("PillarNW");
	private WispData PillarSE = new WispData("PillarSE");
	private WispData PillarSW = new WispData("PillarSW");
	
	private WispData TestWisp = new WispData("Test");
	
	private const int heightFromTerrain = 3;
	
	public GameObject wisp;
	
	// Use this for initialization
	void Start () 
	{
		GateEast.Location = CreateLocation(745, 710);
		GateWest.Location = CreateLocation(490, 660);
		
		Church.Location = CreateLocation(450, 550);
		HousesEast.Location = CreateLocation(545, 590);
		HousesWest.Location = CreateLocation(670, 720);
		
		Graveyard.Location = CreateLocation(490, 600);
		Hill.Location = CreateLocation(770, 590);
		
		ClearingNorth.Location = CreateLocation(560, 820);
		ClearingSouth.Location = CreateLocation(625, 445);
		
		PillarNE.Location = CreateLocation(730, 770);
		PillarNW.Location = CreateLocation(510, 770);
		PillarSE.Location = CreateLocation(510, 480);
		PillarSW.Location = CreateLocation(730, 480);
		
		TestWisp.Location = CreateLocation(630, 630);
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
		var otherTag = other.gameObject.tag;
		
		// I hate doing it like this, but can't use switch
		// as WispData.Tag is not a const D:
		if (GateEast.ValidWisp(otherTag))
		{
			SpawnWisp(other, GateEast);
		}
		else if (GateWest.ValidWisp(otherTag))
		{
			SpawnWisp(other, GateWest);
		}
		else if (Church.ValidWisp(otherTag))
		{
			SpawnWisp(other, Church);
		}
		else if (HousesEast.ValidWisp(otherTag))
		{
			SpawnWisp(other, HousesEast);
		}
		else if (HousesWest.ValidWisp(otherTag))
		{
			SpawnWisp(other, HousesWest);
		}
		else if (Graveyard.ValidWisp(otherTag))
		{
			SpawnWisp(other, Graveyard);
		}
		else if (Hill.ValidWisp(otherTag))
		{
			SpawnWisp(other, Hill);
		}
		else if (ClearingNorth.ValidWisp(otherTag))
		{
			SpawnWisp(other, ClearingNorth);
		}
		else if (ClearingSouth.ValidWisp(otherTag))
		{
			SpawnWisp(other, ClearingSouth);
		}
		else if (PillarNE.ValidWisp(otherTag))
		{
			SpawnWisp(other, PillarNE);
		}
		else if (PillarNW.ValidWisp(otherTag))
		{
			SpawnWisp(other, PillarNW);
		}
		else if (PillarSE.ValidWisp(otherTag))
		{
			SpawnWisp(other, PillarSE);
		}
		else if (PillarSW.ValidWisp(otherTag))
		{
			SpawnWisp(other, PillarSW);
		}
		else
		{
			if(TestWisp.CanSpawn())
			{
				SpawnWisp(other, TestWisp);
			}
		}
	}
	
	/// <summary>
	/// Spawns a wisp that travels toward the location of the wispData.
	/// </summary>
	/// <param name='other'> The collider </param>
	/// <param name='wispData'> The wisp data. </param>
	private void SpawnWisp(Collider other, WispData wispData)
	{
		var tempSpawn = other.gameObject.transform.position;
		tempSpawn.y -= 2;
		
		var w = (GameObject) Instantiate(Resources.Load("Wisp"), tempSpawn, other.gameObject.transform.rotation);
		var wisp = w.GetComponent<RandomWispMovement>();
		
		wisp.end = wispData.Location;
		wispData.LastSpawn = Time.realtimeSinceStartup;
	}
	
	/// <summary>
	/// A class that represents the data of a wisp.
	/// </summary>
	private class WispData
	{
		private const int spawnDifference = 300;
		
		public string Tag { get; private set; }
		public Vector3 Location { get; set; }
		public float LastSpawn { get; set; }
		
		public WispData(string tag)
		{
			Tag = tag;
			Location = new Vector3(0f, 0f, 0f);
			LastSpawn = 0f;
		}
		
		public bool ValidWisp(string tag)
		{
			if (Tag.Equals(tag) && CanSpawn()) return true;
			
			return false;
		}
		
		public bool CanSpawn()
		{
			if (LastSpawn == 0f) return true;	
			if (Time.realtimeSinceStartup - LastSpawn > spawnDifference) return true;
			
			return false;
		}
	}
}
