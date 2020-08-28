using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBRedrawUIScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void CallUIRedraw()
    {
        GameObject gMenuObj = GameObject.Find("FB");
        if (gMenuObj)
        {
            //gMenuObj.GetComponent<FBScript>().RedrawUI();
        }
    }

    public static void MyCallUIRedraw()
    {
        GameObject gMenuObj = GameObject.Find("FB");
        if (gMenuObj)
        {
            gMenuObj.GetComponent<FBScript>().RedrawUI();
        }
    }
}
