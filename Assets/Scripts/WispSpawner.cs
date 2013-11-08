using UnityEngine;
using System.Collections;

public class WispSpawner : MonoBehaviour 
{
	private string GateTag = "Gate";
	private Vector3 GateLocation;
	
	private string ChurchTag = "Church";
	
	private string GraveyardTag = "Graveyard";
	
	private string HillTag = "Hill";
	
	private string ClearingATag = "ClearingA";
	
	private string ClearingBTag = "ClearingB";
	
	public GameObject wisp;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) 
	{	
		//if(other.gameObject.tag == 
		var w = (GameObject) Instantiate(wisp, other.gameObject.transform.position, other.gameObject.transform.rotation);
		var ty = Terrain.activeTerrain.SampleHeight(w.transform.position);
		w.GetComponent<RandomWispMovement>().end = new Vector3(900, ty, 300);
	}
}
