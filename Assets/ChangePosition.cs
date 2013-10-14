using UnityEngine;
using System.Collections;

public class ChangePosition : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		var lt = GameObject.Find("TeaserLight").light.transform;
		lt.transform.localPosition = new Vector3(0, 5, 10);
		var localVector = lt.localPosition;
		
		Vector3 newVector = light.transform.TransformPoint(localVector.x, localVector.y, localVector.z);
		newVector.y = 5;
		lt.position = newVector;
	}
}
