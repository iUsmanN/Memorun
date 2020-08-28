using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningControllerOnPlayer : MonoBehaviour {

    public Light LIGHT;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider A)
    {
        if(A.tag=="HighCube" || A.tag=="LowCube")
        {
            if (DEBUG.DoLOG) Debug.Log("Flash");
            LIGHT.GetComponent<WarningLightControlelr>().Blink();
        }
    }
}
