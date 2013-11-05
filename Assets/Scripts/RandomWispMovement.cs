using UnityEngine;
using System.Collections.Generic;

public class RandomWispMovement : MonoBehaviour {
	
	private bool endReached;
	private List<Vector3> travelPoints = new List<Vector3>();
	private int nextPoint = 1;
	private Vector3 start;
	public float speed = 0.01f;
	public Vector3 end = new Vector3(0, 3, 18);
	
	// Use this for initialization
	void Start () 
	{
		start = transform.position;
		
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
		if(!endReached)
		{
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, travelPoints[nextPoint], step);
			if(transform.position.Equals(travelPoints[nextPoint]))
			{
				nextPoint++;
			}
			
			endReached =  transform.position.Equals(end);
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
}
