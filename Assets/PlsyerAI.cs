using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlsyerAI : MonoBehaviour {

    public Animator Anim;

    void OnTriggerEnter(Collider A)
    {
        if(/*!Anim.GetBool("die") && !Anim.GetBool("dieup")*/true)
        {
            if (DEBUG.DoLOG) Debug.Log("Should Act");
            if (A.gameObject.tag == "LowCube")
                StartCoroutine(JumpUp());

            if (A.gameObject.tag == "HighCube")
                StartCoroutine(JumpDown());
        }
    }

    IEnumerator JumpUp()
    {
        //yield return new WaitForSeconds(0.2f);
        Anim.SetBool("Jump", true);
        yield return new WaitForSeconds(0.2f);
        Anim.SetBool("Jump", false);
    }

    IEnumerator JumpDown()
    {
        //yield return new WaitForSeconds(0f);
        Anim.SetBool("Crouch", true);
        yield return new WaitForSeconds(0.2f);
        Anim.SetBool("Crouch", false);
    }

    // Use this for initialization
    void Start () {
        //Anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
