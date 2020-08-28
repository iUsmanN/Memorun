using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chase : MonoBehaviour
{

    public Transform Player;
    public GameObject AIMain;
    public React player;
    public FadeTextBack SCOREEN;

    //public LevelUP ArcadeObj;

    public bool IsNear;
    Coroutine Shake;
    // Use this for initialization
    void Start()
    {
        IsNear = false;
    }

    IEnumerator ShakeCam()
    {
        while (true)
        {
            Camera.main.transform.DOShakeRotation(5f, new Vector3(0f, 0f, 0.7f));
            yield return new WaitForSeconds(5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.gameObject.GetComponent<Animator>().GetBool("die") && !player.gameObject.GetComponent<Animator>().GetBool("dieup"))
            transform.LookAt(Player.transform);

        else
            transform.DORotate(new Vector3(0f, 0f, 0f), 0.5f);

        if(DEBUG.DoLOG) Debug.Log(Mathf.Abs(Vector3.Distance(Player.transform.position,transform.position)));

        
        if (Mathf.Abs(Vector3.Distance(Player.transform.position, transform.position)) < 1.5f && IsNear)
        {
            if (DEBUG.DoLOG) Debug.LogError("Catch");
            if (!player.gameObject.GetComponent<Animator>().GetBool("die") && !player.gameObject.GetComponent<Animator>().GetBool("dieup"))
            {
                gameObject.GetComponent<Animator>().SetBool("Catch", true);
                player.ChaseCatch();
                //AIMain.GetComponent<MoveAI>().enabled = false;
                
            }
        }
        

            if ((Mathf.Abs(Player.transform.position.x - this.transform.position.x) < 7f) && !IsNear)
        {
            //Shake = StartCoroutine(ShakeCam());
            if (DEBUG.DoLOG) Debug.LogError("Start Chasing");
            IsNear = true;
            StartCoroutine(LevelUP.ChaseCamSetup());
            StartCoroutine(SCOREEN.ShowHint(1));
            //Shake = StartCoroutine(ShakeCam());
        }

        if ((Mathf.Abs(Player.transform.position.x - this.transform.position.x) > 6f) && IsNear)
        {
            if (DEBUG.DoLOG) Debug.LogError("Try Speed AI");
            
            if(AIMain.GetComponent<MoveAI>().incSp>0f)
            {
                StartCoroutine(AIMain.GetComponent<MoveAI>().BoostSP(5f));
            }
        }

        if ((Mathf.Abs(Player.transform.position.x - this.transform.position.x) > 7f) && IsNear && !player.gameObject.GetComponent<Animator>().GetBool("die") && !player.gameObject.GetComponent<Animator>().GetBool("dieup"))
        {
            if (DEBUG.DoLOG) Debug.LogError("Stop Chasing");
            IsNear = false;
            StartCoroutine(LevelUP.ChaseCamOver());
            AIMain.GetComponent<MoveAI>().enabled = false;

            //StopCoroutine(Shake);
        }
    }
}
