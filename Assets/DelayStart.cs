using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStart : MonoBehaviour {

    GameObject[] Mov;

	// Use this for initialization
	void Start () {
        StartCoroutine(DelayIt());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator DelayIt()
    {
        Mov = GameObject.FindGameObjectsWithTag("MP");

        foreach (GameObject g in Mov)
            g.GetComponent<move>().enabled = false;

        yield return new WaitForSeconds(2f);

        foreach (GameObject g in Mov)
            g.GetComponent<move>().enabled = true;
    }
}
