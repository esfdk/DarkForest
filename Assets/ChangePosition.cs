using UnityEngine;
using System.Collections;
 
public class ChangePosition : MonoBehaviour {
        // Update is called once per frame
        void Update () {
                var lt = GameObject.Find("TeaserLight").light.transform;
                lt.transform.localPosition = new Vector3(0, 0.5f, 10);
                var localVector = lt.localPosition;
               
                Vector3 newVector = light.transform.TransformPoint(localVector.x, localVector.y, localVector.z);
                newVector.y = 2;
                lt.position = newVector;
               
                GameObject.Find("Sphere").transform.position = newVector;
               
                var tower = GameObject.Find("Tower").transform;
                var towerVector = new Vector3(0, 0, 100);
                var newTowerVector = light.transform.TransformPoint(towerVector.x, towerVector.y, towerVector.z);
                var towerRotation = tower.rotation;
               
                newTowerVector.y = 50;
                towerRotation.z = 0;
                towerRotation.x = 0;
               
                tower.position = newTowerVector;
                tower.rotation = towerRotation;
        }
}