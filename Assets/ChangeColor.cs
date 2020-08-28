using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    public ColorSystem CS;
    public int ColorCode = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider A)
    {
        if(A.gameObject.tag=="OriginalMainPlayer")
        {
            if(DEBUG.DoLOG) Debug.Log("NOW");
            CS.NewColor(ColorCode);
        }
    }
}
