using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG : MonoBehaviour {

    public static bool DoLOG;
    public bool LogKarun;
    public bool RemovePlayerPrefs;

	// Use this for initialization
	void Start () {
        DoLOG = LogKarun;
        RemPrefs(RemovePlayerPrefs);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void RemPrefs(bool a)
    {
        if (a)
            PlayerPrefs.DeleteAll();
    }
}
