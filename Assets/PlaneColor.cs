using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneColor : MonoBehaviour {

    public Material GroundMat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GroundMat.color = new Color(RenderSettings.fogColor.r * 0.4f, RenderSettings.fogColor.g * 0.4f, RenderSettings.fogColor.b * 0.4f);
	}
}
