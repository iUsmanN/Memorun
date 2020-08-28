using UnityEngine;
using System.Collections;

public class OnGround : MonoBehaviour {

    public static bool OnGroundVar; //Can Jump
    public Animator Anim;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(OnGroundVar);
    }

    void OnCollisionEnter(Collision A)
    {
        OnGroundVar = true;
    }

    void OnCollisionExit(Collision A)
    {
        OnGroundVar = false;
        Anim.SetBool("Jump", false);

    }

    public bool GetGroundVar()
    {
        return OnGroundVar;
    }
}
