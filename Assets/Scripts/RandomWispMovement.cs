using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomWispMovement : MonoBehaviour {
	
	private bool endReached = false;
	private List<Vector3> travelPoints = new List<Vector3>();
	private float despawnDistance = 7;
	private Vector3 start;
	private Hashtable moveParams = new Hashtable();
	
	public float speed;
	public Vector3 end = new Vector3(0, 3, 18);
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () 
	{		
		var tempStart = transform.position;
		tempStart.y = Terrain.activeTerrain.SampleHeight(tempStart) + 1f;
		start = tempStart;
		
		var dist = Vector3.Distance(start, end);
        var numberOfSteps = dist / speed;

        travelPoints.Add(start);

		// Create the initial points of the path.
        for (var i = 0; i < numberOfSteps; i++)
        {
            var current = travelPoints[i];
            travelPoints.Add(Vector3.MoveTowards(current, end, speed));
        }

		// Randomizes each step of the path.
        for (var i = 1; i <= numberOfSteps; i++)
        {
			while(true)
			{
				var v = RandomizeVectorPosition(travelPoints[i]);
				if(Vector3.Distance(v, end) < Vector3.Distance(travelPoints[i-1], end))
				{
					travelPoints[i] = v;
					break;
				}
			}
        }
		
		travelPoints.Add(end);
		
		// Create the params used by iTween.
		moveParams.Add(iT.MoveTo.path, travelPoints.ToArray());
		moveParams.Add(iT.MoveTo.time, dist);
		moveParams.Add(iT.MoveTo.easetype, "linear");
		moveParams.Add(iT.MoveTo.oncomplete, "EndHasBeenReached");
		
		// Move the wisp with iTween.
		iTween.MoveTo(this.gameObject, moveParams);
	}
	
	// Update is called once per frame
	void Update () 
	{		
		// If the end point is reached, but the wisp is not set to disappear yet ...
		if(endReached)
		{
			var pPos = GameObject.Find("Player").transform.position;
			
			// Check if the player is close to the wisp.
			if (Vector3.Distance(pPos, transform.position) < despawnDistance)
			{
				// Add a new end point to the wisp's path
				var tempEnd = end;
				tempEnd.y = Terrain.activeTerrain.SampleHeight(tempEnd) - 2;
				
				var dist = Vector3.Distance(end, tempEnd);
				
				// Create the params used by iTween.
				moveParams = new Hashtable();
				moveParams.Add(iT.MoveTo.path, new Vector3[] {end, tempEnd});
				moveParams.Add(iT.MoveTo.time, dist * 2);
				moveParams.Add(iT.MoveTo.easetype, "linear");
				moveParams.Add(iT.MoveTo.oncomplete, "DestroyWisp");
				
				// Move the wisp with iTween.
				iTween.MoveTo(this.gameObject, moveParams);
				
				endReached = false;
			}
		}
	}

	/// <summary>
	/// Tells the wisp that the end has been reached.
	/// </summary>
	void EndHasBeenReached()
	{
		endReached = true;
	}

	/// <summary>
	/// Destroys the wisp.
	/// </summary>
	void DestroyWisp()
	{
		Destroy (this);
	}

	/// <summary>
	/// Randomizes the given vector position.
	/// </summary>
	/// <returns> A randomized vector. </returns>
	/// <param name="vector"> The vector to randomize. </param>
	Vector3 RandomizeVectorPosition(Vector3 vector)
	{	
		var newX = Random.Range(vector.x - 1f, vector.x + 1f);
		var newY = Random.Range(vector.y - 1f, vector.y + 1f);
		var newZ = Random.Range(vector.z - 1f, vector.z + 1f);
		
		var newVector = new Vector3(newX, newY, newZ);
		
		var ty = Terrain.activeTerrain.SampleHeight(newVector);
		
		if(newVector.y < ty)
		{
			newVector.y = ty + 0.9f;
		}
		
		return newVector;
	}

	/// <summary>
	/// Sets the end position for the wisp.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	void SetEnd(float x, float y, float z)
	{
		end.x = x;
		end.y = y;
		end.z = z;
	}
}
