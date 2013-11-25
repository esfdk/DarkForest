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
	
	// Use this for initialization
	void Start () 
	{		
		var tempStart = transform.position;
		tempStart.y = Terrain.activeTerrain.SampleHeight(tempStart) + 1f;
		start = tempStart;
		
		var dist = Vector3.Distance(start, end);

        var numberOfSteps = dist / speed;

        travelPoints.Add(start);

        for (var i = 0; i < numberOfSteps; i++)
        {
            var current = travelPoints[i];
            travelPoints.Add(Vector3.MoveTowards(current, end, speed));
        }

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
		moveParams.Add(iT.MoveTo.time, dist * 2);
		moveParams.Add(iT.MoveTo.easetype, "linear");
		moveParams.Add(iT.MoveTo.oncomplete, "EndHasBeenReached");
		
		// Move the wisp with iTween.
		iTween.MoveTo(this.gameObject, moveParams);
	}
	
	void EndHasBeenReached()
	{
		endReached = true;
	}
	
	void DestroyWisp()
	{
		Destroy (this);
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
	
	Vector3 RandomizeVectorPosition(Vector3 v)
	{	
		var newX = Random.Range(v.x - 1f, v.x + 1f);
		var newY = Random.Range(v.y - 1f, v.y + 1f);
		var newZ = Random.Range(v.z - 1f, v.z + 1f);
		
		var vector = new Vector3(newX, newY, newZ);
		
		var ty = Terrain.activeTerrain.SampleHeight(vector);
		
		if(vector.y < ty)
		{
			vector.y = ty + 0.9f;
		}
		
		
		return vector;
	}
	
	void SetEnd(float x, float y, float z)
	{
		end.x = x;
		end.y = y;
		end.z = z;
	}
}
