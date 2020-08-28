using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeScreenCubes : MonoBehaviour {

    public float Speed = 10f;

	// Use this for initialization
	void Start () {
        Speed *= -1f;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(-10f * Time.deltaTime, 0f, 0f);

        /*
        if (transform.position.x < -245f)
            transform.position = new Vector3(-95f, transform.position.y, transform.position.z);
            */
	}
}
