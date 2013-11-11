using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WispSpawner : MonoBehaviour 
{	
	private System.Collections.Generic.List<WispData> WispList;
	
	/// <summary>
	/// Constant height from terrain. (used by the WispData class)
	/// </summary>
	private const int heightFromTerrain = 3;
	
	public GameObject wisp;
	
	// Use this for initialization
	void Start () 
	{
		WispList = new System.Collections.Generic.List<WispData>();
		
		WispList.Add(new WispData("GateEast", 745, 710));
		WispList.Add(new WispData("GateWest", 490, 660));
		
		WispList.Add(new WispData("Church", 450, 550));
		WispList.Add(new WispData("HousesEast", 545, 590));
		WispList.Add(new WispData("HousesWest", 670, 720));
		
		WispList.Add(new WispData("Graveyard", 490, 600));
		WispList.Add(new WispData("Hill", 770, 590));
		WispList.Add(new WispData("Well", 560, 715));
		
		WispList.Add(new WispData("ClearingNorth", 560, 820));
		WispList.Add(new WispData("ClearingSouth", 625, 445));
		
		WispList.Add(new WispData("PillarNE", 730, 770));
		WispList.Add(new WispData("PillarNW", 510, 770));
		WispList.Add(new WispData("PillarSE", 510, 480));
		WispList.Add(new WispData("PillarSW", 730, 480));
		
		WispList.Add(new WispData("StatueNorth", 420, 690));
		WispList.Add(new WispData("StatueSouth", 420, 630));
		
		WispList.Add(new WispData("Test", 630, 630));
	}
	
	// Update is called once per frame
	void Update () 
	{
		foreach (var wisp in WispList)
		{
			if (wisp.SpawningCycleOngoing())
			{
				wisp.SpawnWisp();	
			}
		}
	}
	
	void OnTriggerEnter(Collider other) 
	{	
		var otherTag = other.gameObject.tag;
		otherTag = "Test";
		var tempWisp = WispList.FirstOrDefault(a => a.Tag == otherTag);
		
		if (tempWisp != null)
		{
			if (tempWisp.CanStartNewSpawnCycle())
			{				
				var tempPlayer = GameObject.Find("Player").transform;
				
				var tempPosition = new Vector3(tempPlayer.position.x, tempPlayer.position.y, tempPlayer.position.z);
				var tempRotation = new Quaternion(tempPlayer.rotation.x, tempPlayer.rotation.y, tempPlayer.rotation.z, tempPlayer.rotation.w);
				
				var spawnPosition = tempPosition;
				var spawnRotation = tempRotation;
				
				tempWisp.StartSpawning(3, spawnPosition, spawnRotation);
			}
		}
	}
	
	/// <summary>
	/// A class that represents the data of a wisp.
	/// </summary>
	private class WispData
	{
		private const int spawnCycleDifference = 300;
		
		public string Tag { get; private set; }
		public Vector3 EndLocation { get; set; }
		public float LastSpawnCycle { get; set; }
		
		private bool firstWisp, playSound;
		private int wispsToSpawn;
		private float lastSpawn, spawnDelay = 5f;
		private Vector3 spawnLocation;
		private Quaternion spawnRotation;
		
		public WispData(string tag, int x, int z)
		{
			Tag = tag;
			CreateLocation(x, z);
			LastSpawnCycle = 0f;
			
			wispsToSpawn = 0;
		}
		
		/// <summary>
		/// Determines whether the wisp is allowed to start a new cycle of spawning.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can spawn; otherwise, <c>false</c>.
		/// </returns>
		public bool CanStartNewSpawnCycle()
		{
			if (LastSpawnCycle == 0f) return true;	
			if (Time.realtimeSinceStartup - LastSpawnCycle > spawnCycleDifference) return true;
			
			return false;
		}
		
		/// <summary>
		/// Determines whether a spawning cycle is in progress..
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance is spawning; otherwise, <c>false</c>.
		/// </returns>
		public bool SpawningCycleOngoing()
		{
			return wispsToSpawn > 0;
		}
		
		/// <summary>
		/// Starts a cycle of spawning.
		/// </summary>
		/// <param name='numberofWisps'> The amount of wisps to spawn for the cycle. </param>
		/// <param name='newSpawnLocation'> The spawn location. </param>
		/// <param name='newSpawnRotation'> The spawn rotation. </param>
		public void StartSpawning(int numberofWisps, Vector3 newSpawnLocation, Quaternion newSpawnRotation)
		{
			spawnLocation.y = Terrain.activeTerrain.SampleHeight(spawnLocation) - 2;
			
			LastSpawnCycle = Time.realtimeSinceStartup;
			
			firstWisp = true;
			wispsToSpawn = numberofWisps;
			spawnLocation = newSpawnLocation;
			spawnRotation = newSpawnRotation;
			lastSpawn = Time.realtimeSinceStartup;
		}
	
		/// <summary>
		/// Spawns a wisp that travels toward the location of the wispData.
		/// </summary>
		/// <param name='wispData'> The wisp data. </param>
		public void SpawnWisp()
		{			
			if (SpawnWispInCycle())
			{
				if (playSound)
				{
					AudioSource.PlayClipAtPoint((AudioClip) Resources.Load("Wisp/Wisp spawn"), spawnLocation);
					playSound = false;
				}
				
				var w = (GameObject) Instantiate(Resources.Load("Wisp/Wisp"), spawnLocation, spawnRotation);
				var wisp = w.GetComponent<RandomWispMovement>();
				
				wisp.end = EndLocation;
				
				wispsToSpawn--;
			}
		}
		
		/// <summary>
		/// Determines if it is time to spawn a wisp in the current cycle.
		/// </summary>
		/// <returns> 
		/// <c>true</c> if iit is time; otherwise, <c>false</c>.
		/// </returns>
		private bool SpawnWispInCycle()
		{
			if (!SpawningCycleOngoing()) return false;
			
			if (firstWisp)
			{
				firstWisp = false; 
				playSound = true;
				lastSpawn = Time.realtimeSinceStartup;
				return true;
			}
			
			if (Time.realtimeSinceStartup - lastSpawn > spawnDelay) 
			{
				lastSpawn = Time.realtimeSinceStartup; 
				return true;
			}
			
			return false;
		}
	
		/// <summary>
		/// Creates a location with the terrain's height based on the x and z coordinates.
		/// </summary>
		/// <returns> The location. </returns>
		/// <param name='x'> The x-coordinate. </param>
		/// <param name='z'> The z-coordinate. </param>
		private void CreateLocation(int x, int z)
		{
			var tempVector = new Vector3(x, 0, z);
			var terrainY = Terrain.activeTerrain.SampleHeight(tempVector) + heightFromTerrain;
			tempVector.y = terrainY;
			
			EndLocation = tempVector;
		}
	}
}
