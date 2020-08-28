using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICheckPoint : MonoBehaviour {
    public GameObject AIPlayer, Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(DEBUG.DoLOG) Debug.LogWarning(Vector3.Distance(this.transform.position, Player.gameObject.transform.position) < Vector3.Distance(this.transform.position, AIPlayer.transform.position));
	}

    void OnTriggerEnter(Collider A)
    {
        if(A.gameObject.tag == "OriginalMainPlayer")
        {

            if(DEBUG.DoLOG) Debug.LogWarning("CheckP");
            PROGRESS.a++;


            if(Vector3.Distance(this.transform.position, A.gameObject.transform.position)<Vector3.Distance(this.transform.position, AIPlayer.transform.position))
                PROGRESS.c++;
        }
    }
}
