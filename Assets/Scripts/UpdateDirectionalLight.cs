using UnityEngine;
using System.Collections;

public class UpdateDirectionalLight : MonoBehaviour 
{
	// The speed at which the light changes
	private float ChangeSpeed = 0.01f;
	
	// The middle of the map.
	private Vector3 midLocation = new Vector3(625, 0, 625);
	
	// Colours
	private Color32 LightBlack = Color.gray;
	private Color32 LightBlue = Color.blue;
	private Color32 LightPurple = new Color32(159, 0, 197, 255);
	private Color32 LightRed = Color.red;
	private Color32 LightYellow = Color.yellow;
	private Color32 LightGreen = Color.green;
	
	// Skybox colors
	private Color32 SkyboxBlack = new Color32 (110, 110, 110, 255);
	private Color32 SkyboxBlue = new Color32 (120, 120, 140, 255);
	private Color32 SkyboxPurple = new Color32 (130, 0, 130, 255);
	private Color32 SkyboxRed = new Color32 (150, 130, 130, 255);
	private Color32 SkyboxYellow = new Color32 (140, 140, 0, 255);
	private Color32 SkyboxGreen = new Color32 (130, 150, 130, 255);
	
	// Fog colors
	private Color32 FogBlack = new Color32 (5, 5, 5, 255);
	private Color32 FogBlue = new Color32 (80, 80, 140, 255);
	private Color32 FogPurple = new Color32 (140, 0, 140, 255);
	private Color32 FogRed = new Color32 (140, 80, 80, 255);
	private Color32 FogYellow = new Color32 (125, 125, 0, 255);
	private Color32 FogGreen = new Color32 (130, 150, 130, 255);
	
	// Transforms
	private Transform pTransform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		pTransform = transform;
		
	    var pPos  = pTransform.position;
		var xDist = pPos.x - midLocation.x;
		var zDist = pPos.z - midLocation.z;
		var tDist = Mathf.Sqrt(Mathf.Pow(xDist, 2f) + 0 + Mathf.Pow(zDist, 2f));
		
		if (tDist < 15) 		{ ChangeLight(0.3f, LightBlack, SkyboxBlack, FogBlack); }
		else if (tDist < 125) 	{ ChangeLight(0.4f, LightBlue, SkyboxBlue, FogBlue); }
		else if (tDist < 250) 	{ ChangeLight(0.5f, LightPurple, SkyboxPurple, FogPurple); }
		else if (tDist < 375) 	{ ChangeLight(0.6f, LightRed, SkyboxRed, FogRed); }
		else if (tDist < 500) 	{ ChangeLight(0.7f, LightYellow, SkyboxYellow, FogYellow); }
		else if (tDist < 625) 	{ ChangeLight(0.8f, LightGreen, SkyboxGreen, FogGreen); }
	}
	
	/// <summary>
	/// Changes the directional light.
	/// </summary>
	/// <param name='targetLight'> Target light intensity. </param>
	/// <param name='newColor'> The new color for the directional light. </param>
	/// <param name='skyboxColor'> The new color for the skybox. </param>
	/// <param name='fogColor'> The new color for the fog. </param>
	private void ChangeLight(float targetLight, Color newColor, Color skyboxColor, Color fogColor)
	{
		var light = GameObject.Find("MainLight").light;
		var tempColor = Color.Lerp (light.color, newColor, ChangeSpeed);
		
		if (light.intensity > targetLight) { light.intensity -= ChangeSpeed; }
		if (light.intensity < targetLight) { light.intensity += ChangeSpeed; }
		
		var newAngle = light.transform.eulerAngles;
		newAngle.y = pTransform.eulerAngles.y;
		
		light.transform.eulerAngles = newAngle;
		light.color = tempColor;
		
		RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, fogColor, ChangeSpeed);
		RenderSettings.skybox.SetColor("_Tint", Color.Lerp(RenderSettings.skybox.GetColor("_Tint"), skyboxColor, ChangeSpeed));
	}
}
