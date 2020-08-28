using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour {

    MeshRenderer M;

	// Use this for initialization
	void Start () {
        M = gameObject.GetComponent<MeshRenderer>();
        //StartCoroutine(Flicker());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator Flicker()
    {
        while (true)
        {
            M.enabled = false;
            yield return new WaitForSeconds(0.04f);
            M.enabled = true;
            yield return new WaitForSeconds(0.02f);
            M.enabled = false;
            yield return new WaitForSeconds(0.02f);
            M.enabled = true;
        }
    }
}
