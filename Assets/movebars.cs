using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movebars : MonoBehaviour {
    public float Sp = -15f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(0f, 0f, Random.Range(Sp, Sp-15f) * Time.deltaTime));

        if (transform.position.z < -23f)
            transform.position = new Vector3(transform.position.x, transform.position.y, 135f);
	}
}
