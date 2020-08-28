using UnityEngine;
using System.Collections;

public class endgame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Application.LoadLevel(0);
            if (DEBUG.DoLOG) Debug.Log("change scene");
        }
	}

    void OnCollisionEnter(Collision A)
    {
        if (DEBUG.DoLOG) Debug.Log("Can ENDGAME");
        if (A.gameObject.tag=="LowCube")
            if (DEBUG.DoLOG) Debug.Log("ENDGAME");
    }
}
