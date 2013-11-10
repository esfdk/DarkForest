using UnityEngine;
using System.Collections.Generic;

public class RandomWispMovement : MonoBehaviour {
	
	private bool endReached, wispDisappear = false;
	private List<Vector3> travelPoints = new List<Vector3>();
	private int nextPoint = 1;
	private float pauseTimer;
	private float despawnDistance = 15;
	private Vector3 start;
	
	public float speed;
	public Vector3 end = new Vector3(0, 3, 18);
	public int pauseProbability = 10;
	public float pauseLength = 1.0f;
	
	// Use this for initialization
	void Start () 
	{
		var tempStart = transform.position;
		tempStart.y = Terrain.activeTerrain.SampleHeight(tempStart) + 2;
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(pauseTimer > 0)
		{
			pauseTimer -= Time.deltaTime;
			return;
		}
		
		if(!endReached)
		{
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, travelPoints[nextPoint], step);
			
			if(transform.position.Equals(travelPoints[nextPoint]))
			{
				if(Random.Range(0, 100) <= pauseProbability)
				{
					pauseTimer = pauseLength;	
				}
				
				nextPoint++;
			}
			
			endReached =  transform.position.Equals(end);
		}
		
		// If the end point is reached, but the wisp is not set to disappear yet ...
		if(endReached && !wispDisappear)
		{
			var pPos = GameObject.Find("Player").transform.position;
			
			// Check if the player is close to the wisp.
			if (Vector3.Distance(pPos, transform.position) < despawnDistance)
			{
				// Add a new end point to the wisp's path
				var tempEnd = end;
				tempEnd.y = Terrain.activeTerrain.SampleHeight(tempEnd) - 2;
				
				travelPoints[nextPoint] = tempEnd;
				end = tempEnd;
				
				// Make sure this part of code isn't called again, but force the wisp
				// to move again.
				wispDisappear = true;
				endReached = false;
			}
		}
		
		// If the wisp has gone underground ...
		if(endReached && wispDisappear)
		{
			// .. destroy it.
			Destroy(this);
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
			vector.y = ty + 0.2f;
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
