using UnityEngine;
using System.Collections;

public class Player_RespawnScript : MonoBehaviour 
{

	private Vector3 spawnPoint;
	private Vector3 spawnRotation;

	private float lastSpawn = 0f;

	private bool activated;

	// Use this for initialization
	void Start () 
	{
//		spawnPoint = this.transform.position;
//		spawnPoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
		spawnPoint = new Vector3(619, 1, 610);
		spawnRotation = this.transform.eulerAngles;

		lastSpawn = Time.realtimeSinceStartup;

		activated = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.realtimeSinceStartup - lastSpawn < 2f)
		{
			if (activated)
			{
				this.GetComponent<CharacterController>().enabled = false;
				this.GetComponent<MouseLook>().enabled = false;
				this.transform.FindChild("Main Camera").gameObject.GetComponent<MouseLook>().enabled = false;

				activated = false;
			}
		}
		else
		{
			if (!activated)
			{
				this.GetComponent<CharacterController>().enabled = true;
				this.GetComponent<MouseLook>().enabled = true;
				this.transform.FindChild("Main Camera").gameObject.GetComponent<MouseLook>().enabled = true;

				activated = true;
			}
		}
	}
	
	public void Respawn()
	{
		this.GetComponent<CharacterController>().enabled = false;
		this.GetComponent<MouseLook>().enabled = false;
		this.transform.FindChild("Main Camera").gameObject.GetComponent<MouseLook>().enabled = false;

		this.transform.position = spawnPoint;
		this.transform.Rotate(spawnRotation.x - transform.eulerAngles.x, 
		                      spawnRotation.y - transform.eulerAngles.y, 
		                      spawnRotation.z - transform.eulerAngles.z);
		lastSpawn = Time.realtimeSinceStartup;

		this.GetComponent<CharacterController>().enabled = true;
		this.GetComponent<MouseLook>().enabled = true;
		this.transform.FindChild("Main Camera").gameObject.GetComponent<MouseLook>().enabled = true;
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "TowerCollider")
		{
			this.Respawn();
		}
	}
}
