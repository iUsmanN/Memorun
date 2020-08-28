using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SettingMyNetwork : NetworkBehaviour {

    Lean.Touch.LeanSwipeDirection4 A;
    Menu M;

    public bool AmILocal()
    {
        return isLocalPlayer;
    }

    // Use this for initialization
    void Start()
    {

        if (isLocalPlayer)
        {
            transform.position = new Vector3(transform.position.x - 20f, transform.position.y, transform.position.z);

            A = GameObject.FindGameObjectWithTag("Lean").GetComponent<Lean.Touch.LeanSwipeDirection4>();
            A.Char = this.GetComponent<Rigidbody>();
            A.CharHere = this.gameObject.GetComponent<OnGround>();
            A.Anim = A.Char.gameObject.GetComponent<Animator>();
            A.ReactS = A.Char.gameObject.GetComponent<React>();
            A.ReactS.Cam = Camera.main;

            this.gameObject.GetComponent<React>().MOVE = this.gameObject.GetComponent<move>();

            if (GameObject.FindGameObjectWithTag("Controller").GetComponent<move>())
            {
                GameObject.FindGameObjectWithTag("Controller").GetComponent<move>().Char = this.gameObject.GetComponent<Transform>();
                GameObject.FindGameObjectWithTag("Controller").GetComponent<move>().enabled = true;
                

                M = GameObject.FindGameObjectWithTag("MenuTag").GetComponent<Menu>();
                M.StartCoroutine(M.CamPos());
                M.StartCoroutine(M.SpeedUp(2));

                GetComponent<React>().Quote = GameObject.FindGameObjectWithTag("replte").GetComponent<Text>();
                GetComponent<React>().MOVE = GameObject.FindGameObjectWithTag("Controller").GetComponent<move>();

                if (GameObject.FindGameObjectWithTag("rt"))
                {
                    GetComponent<React>().RT = GameObject.FindGameObjectWithTag("rt");
                    GetComponent<React>().ReplayC = GameObject.FindGameObjectWithTag("rt").GetComponent<Canvas>();
                    GameObject.FindGameObjectWithTag("rt").SetActive(false);
                }

                //GetComponent<React>().Menu = GameObject.FindGameObjectWithTag("MenuTag");

                Debug.Log("Set Cam");
                GameObject.FindGameObjectWithTag("MainCamera").transform.parent = this.transform.Find("CamLoc");
                GameObject.FindGameObjectWithTag("MainCamera").transform.rotation = this.transform.Find("CamLoc").rotation;
                GameObject.FindGameObjectWithTag("MainCamera").transform.position = this.transform.Find("CamLoc").position;

            }
            

            ArcadeRs.Player = this.transform;
            Debug.Log("ArcadeRs.Player = " + this.gameObject.name);

            
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isLocalPlayer)
        {
            if (Camera.main.transform.parent == null)
            {
                Debug.Log("Set Cam");
                GameObject.FindGameObjectWithTag("MainCamera").transform.parent = this.transform.Find("CamLoc");
                GameObject.FindGameObjectWithTag("MainCamera").transform.rotation = this.transform.Find("CamLoc").rotation;
                GameObject.FindGameObjectWithTag("MainCamera").transform.position = this.transform.Find("CamLoc").position;
            }
        }

        if(A.ReactS.Cam == null)
            A.ReactS.Cam = Camera.main;


        if (isLocalPlayer && ArcadeRs.Player!=this.transform)
            ArcadeRs.Player = this.transform;
    }
}