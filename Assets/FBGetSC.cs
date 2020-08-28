using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;

public class FBGetSC : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void QueryScores() //Gets scores of friends
    {
        FB.API("/app/scores?fields=score,user.limit(10)", HttpMethod.GET, ScoresCallBack);
    }

    private void ScoresCallBack(IGraphResult result)
    {
        //Debug.Log(result.text);
        throw new NotImplementedException();
    }
}
