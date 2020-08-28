using UnityEngine;
using System.Collections;

public class buildingscroll : MonoBehaviour
{

    public Camera Cam;
    GameObject[] BKs;
    Vector3 a, low, up;
    // Use this for initialization
    void Start()
    {
        //BKs=GameObject.FindGameObjectsWithTag("BackCube");
        //low = Cam.ScreenToWorldPoint(new Vector3(0, 0));
        a = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Cam)
            Cam = Camera.main;
        if (Cam)
        {
            low = Cam.ScreenToWorldPoint(new Vector3(0, 0));
            up = Cam.ScreenToWorldPoint(new Vector3(Cam.pixelWidth, Cam.pixelHeight, 0f));
            //if(DEBUG.DoLOG) Debug.Log(low.x);
            if (transform.position.x < low.x - 40f)
            {
                if(DEBUG.DoLOG) Debug.Log("OUT");
                transform.position = new Vector3(transform.position.x + Random.Range(200f, 210f), a.y, a.z);
            }
        }
    }
}
