using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OKAYNOWIAMDOINGSOMETHINGWITHNETWORK : NetworkBehaviour {

    Lean.Touch.LeanSwipeDirection4 A;

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            A = GameObject.FindGameObjectWithTag("Lean").GetComponent<Lean.Touch.LeanSwipeDirection4>();
            A.Char = this.GetComponent<Rigidbody>();
            A.CharHere = this.gameObject.GetComponent<OnGround>();
            A.Anim = this.gameObject.GetComponent<Animator>();
            A.ReactS = this.gameObject.GetComponent<React>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
