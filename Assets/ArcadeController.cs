using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArcadeController : MonoBehaviour
{
    public GameObject LastArcadeBlock;
    public static GameObject Last;
    public ArcadeVibrateSc Spawner;

    public GameObject[] Obstacles = new GameObject[5];

    Coroutine Vibrate;

    float a;

    // Use this for initialization
    void Start()
    {
        
        Last = LastArcadeBlock;

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider A)
    {
        if (A.gameObject.tag == "OriginalMainPlayer")
        {
            a = Random.Range(0f, 10f);
            /*
            if (a <= 3f)
            {
                if(DEBUG.DoLOG) Debug.Log("Chotay, ghoomnay walay obstacles");

                foreach(GameObject g in Obstacles)
                {
                    g.transform.
                    
            Z(19f, );

                    
                }

                ArcadeRs.MinDist = Random.Range(8f, 14f);
                ArcadeRs.MaxDist = Random.Range(14f, 16f);
            }

            else
            {
                */
                if(DEBUG.DoLOG) Debug.Log("Normal, desi obstacles");
                ArcadeRs.MinDist = Random.Range(10f, 14f);
                ArcadeRs.MaxDist = Random.Range(14f, 19f);
            //}
            if(DEBUG.DoLOG) Debug.Log("Reset Spawning Positions");
        }
    }
}
