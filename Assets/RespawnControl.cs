using UnityEngine;
using System.Collections;

public class RespawnControl : MonoBehaviour {

    public Vector3 T;

	// Use this for initialization
	void Start () {
        T = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RespawnNow()
    {
        gameObject.transform.position = T;
        if (DEBUG.DoLOG) Debug.Log("Respawning");
    }
}
