using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PROGRESS : MonoBehaviour {

    public move m;
    public static float a;
    public static float b;
    public static float c;
    public Animator Anim;
    public Text SCORE;
    public Transform Strt, End, Player;
    public Menu menu;

    public static bool ai;

	// Use this for initialization
	void Start () {
        b = 5f;                 //total checkpoints
        a = 0;                  //my score
        c = 0f;                 //checkpoints lead
        ai = false;
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(a.ToString() + ", " + b.ToString() + ", " + c.ToString());

        if(ai)
        {
            if (DEBUG.DoLOG) Debug.Log("ai Score");
			SCORE.text = "-No  deaths\n\n-Infinite  run";//a.ToString() + "/" + b.ToString();
			SCORE.fontSize = SCORE.fontSize/2;
        }

        else if (GameObject.FindGameObjectWithTag("RainyMemory"))
        {
            if (DEBUG.DoLOG) Debug.Log("% Score");
            a = Mathf.Ceil(((Player.position.x - Strt.position.x) / (End.position.x - Strt.position.x)) * 100);
            SCORE.text = a.ToString() + '%';
        }

        else if (GameObject.FindGameObjectWithTag("Arcade") && !(Anim.GetBool("die") || Anim.GetBool("dieup")))
        {
            //Debug.Log("m Score");
            if (menu.isAIMode)
            {
                
            }

            else
            {
                a += Time.deltaTime * (m.Sp / 8.2f) * 2f;
                SCORE.text = Mathf.Ceil(a).ToString() + 'm';
            }
        }

        //Debug.Log("Prog : " + a);
	}
}
