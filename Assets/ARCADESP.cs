﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCADESP : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.timeScale!=1.15f)
        Time.timeScale = 1.15f;
	}
}
