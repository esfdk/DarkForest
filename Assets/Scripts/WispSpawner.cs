using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class WispSpawner : MonoBehaviour 
{	
	private Transform player;
	
	private System.Collections.Generic.List<WispData> WispList;
	
	/// <summary>
	/// Constant height from terrain. (used by the WispData class)
	/// </summary>
	private const int heightFromTerrain = 3;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{ 
		player = GameObject.Find("Player").transform;

		// Create the wisp locations.
		WispList = new System.Collections.Generic.List<WispData>();
		
		WispList.Add(new WispData("GateEast", 765, 712));
		
		WispList.Add(new WispData("Church", 475, 560));
		WispList.Add(new WispData("HousesEast", 684, 722));
		WispList.Add(new WispData("HousesWest", 525, 615));
		
		WispList.Add(new WispData("Graveyard", 490, 600));
		WispList.Add(new WispData("Hill", 760, 615));
		WispList.Add(new WispData("Well", 560, 715));
		
		WispList.Add(new WispData("ClearingNorth", 560, 820));
		WispList.Add(new WispData("ClearingSouth", 625, 445));
		
		WispList.Add(new WispData("Statues", 420, 660));

		WispList.Add(new WispData("Field", 218, 493));
		WispList.Add(new WispData("PlusClearing", 436, 147));
		WispList.Add(new WispData("StarClearing", 970, 674));
		WispList.Add(new WispData("StoneHenge", 1060, 753));

		WispList.Add(new WispData("BaronCastle", 854, 366));
		WispList.Add(new WispData("Amphitheater", 859, 964));
		WispList.Add(new WispData("Tree", 873, 809));
	}

	/// <summary>
	/// Update this instance (called once per frame).
	/// </summary>
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

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other"> The object being collided with. </param>
	void OnTriggerEnter(Collider other) 
	{	
		var otherTag = other.gameObject.tag;
		var tempWisp = WispList.FirstOrDefault(a => a.Tag == otherTag);
		
		if (tempWisp != null)
		{
			if (tempWisp.CanStartNewSpawnCycle())
			{				
				var tempPosition = new Vector3(player.position.x, player.position.y, player.position.z) + player.forward * 10;
				var tempRotation = new Quaternion(player.rotation.x, player.rotation.y, player.rotation.z, player.rotation.w);
				
				var spawnPosition = tempPosition;
				var spawnRotation = tempRotation;
				
				tempWisp.StartSpawning(5, spawnPosition, spawnRotation);
			}
		}
	}
	
	/// <summary>
	/// A class that represents the data of a wisp.
	/// </summary>
	private class WispData
	{
		private const int spawnCycleDifference = 120;
		
		public string Tag { get; private set; }
		
		private bool firstWisp, playSound;
		private int wispsToSpawn;
		private float lastSpawn, lastSpawnCycle, spawnDelay = 5f;
		private Vector3 spawnLocation;
		private Vector3 endLocation;
		private Quaternion spawnRotation;
		
		public WispData(string tag, int x, int z)
		{
			Tag = tag;
			CreateLocation(x, z);
			lastSpawnCycle = 0f;
			
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
			if (lastSpawnCycle == 0f) return true;	
			if (Time.realtimeSinceStartup - lastSpawnCycle > spawnCycleDifference) return true;
			
			return false;
		}
		
		/// <summary>
		/// Starts a cycle of spawning.
		/// </summary>
		/// <param name='numberofWisps'> The amount of wisps to spawn for the cycle. </param>
		/// <param name='newSpawnLocation'> The spawn location. </param>
		/// <param name='newSpawnRotation'> The spawn rotation. </param>
		public void StartSpawning(int numberofWisps, Vector3 newSpawnLocation, Quaternion newSpawnRotation)
		{
			spawnLocation.y = Terrain.activeTerrains[8].SampleHeight(spawnLocation) - 2;
			
			lastSpawnCycle = Time.realtimeSinceStartup;
			
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
				// Plays the wisp sound.
				if (playSound)
				{
					AudioSource.PlayClipAtPoint((AudioClip) Resources.Load("Wisp/Wisp spawn"), spawnLocation);
					playSound = false;
				}

				// Instantiazes the wisp.
				var w = (GameObject) Instantiate(Resources.Load("Wisp/Wisp"), spawnLocation, spawnRotation);
				var wisp = w.GetComponent<RandomWispMovement>();
				
				wisp.end = endLocation;
				
				wispsToSpawn--;
			}
		}
		
		/// <summary>
		/// Determines if it is time to spawn a wisp in the current cycle.
		/// </summary>
		/// <returns> 
		/// <c>true</c> if it is time; otherwise, <c>false</c>.
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
		/// Creates a location with the terrain's height based on the x and z coordinates.
		/// </summary>
		/// <returns> The location. </returns>
		/// <param name='x'> The x-coordinate. </param>
		/// <param name='z'> The z-coordinate. </param>
		private void CreateLocation(int x, int z)
		{
			var tempVector = new Vector3(x, 0, z);
			var terrainY = Terrain.activeTerrains[8].SampleHeight(tempVector) + heightFromTerrain;
			tempVector.y = terrainY;
			
			endLocation = tempVector;
		}
	}
}
