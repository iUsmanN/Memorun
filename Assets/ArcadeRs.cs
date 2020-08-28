using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArcadeRs : MonoBehaviour
{
    public static float c = 2f;
    public static float cCh = 2f;
    public static float dist = 17f;
    public Transform RespawnLocation;
    public static Transform Player;

    public static float MinDist = 15f;
    public static float MaxDist = 17f;

    // Use this for initialization
    void Start()
    {
        c = 0f;
        if (GameObject.FindGameObjectWithTag("OriginalMainPlayer"))
            Player = GameObject.FindGameObjectWithTag("OriginalMainPlayer").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if(DEBUG.DoLOG) Debug.Log("c : " + c + ", cCh : " + cCh);

        //if(DEBUG.DoLOG) Debug.Log(dist);
        if(!Player)
        {
            Player = Camera.main.transform;
        }

        if (Player)
        {
            if (RespawnLocation)
            {
                if (transform.position.x < Player.position.x - 10f)
                {
                    c++;

                    Random.InitState(System.DateTime.Now.Millisecond);
                    if(DEBUG.DoLOG) Debug.Log("Cube ko aasmaan par rakhne laga");
                    transform.position = new Vector3(ArcadeController.Last.transform.position.x + Random.Range(MinDist, MaxDist), 54f, transform.position.z);
                    if(DEBUG.DoLOG) Debug.Log("Cube ko aasmaan par rakh dia");
                    if(DEBUG.DoLOG) Debug.Log("RESPW-LOC");
                    RandomHighLow();
                    ArcadeController.Last = this.gameObject;
                    LevelUP.Last = this.gameObject;
                }
            }
            else
            {
                if (transform.position.x < Player.position.x - 10f)
                {
                    c++;// ChangeMode();
                    transform.position = new Vector3(transform.position.x + (dist*5f), transform.position.y, transform.position.z);
                    RandomHighLow();
                    if(DEBUG.DoLOG) Debug.Log("RESPW");
                }
            }
        }
    }

    void RandomHighLow()
    {
        if(Random.Range(0f,10f) < 5f)
        {
            if(DEBUG.DoLOG) Debug.Log("Low");
            //transform.position = new Vector3(transform.position.x, -1.1f, transform.position.z);
            transform.DOMoveY(-1.1f, 5f);
            //if(DEBUG.DoLOG) Debug.Log("Cube ko low cube bana rha");
            transform.gameObject.tag = "LowCube";
        }
        else
        {
            if(DEBUG.DoLOG) Debug.Log("High");
            //transform.position = new Vector3(transform.position.x, 1.0625f, transform.position.z);
            transform.DOMoveY(1.0625f, 5f);
            //if(DEBUG.DoLOG) Debug.Log("Cube ko high cube bana rha");
            transform.gameObject.tag = "HighCube";
        }
    }

    void ChangeMode()
    {
        if(c>cCh)
        {
            c -= 5f;
            cCh += Random.Range(1f, 3f) * 5f;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 30f);
            gameObject.GetComponent<Rotate>().enabled = true;
        }

        else if(transform.localScale.z < 60f)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 178f);
            gameObject.GetComponent<Rotate>().enabled = false;
        }
    }
}
