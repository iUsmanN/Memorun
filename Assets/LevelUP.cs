using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelUP : MonoBehaviour {

    public GameObject AI, PlayersLowmanShoes, Respawner, AIMain;

    public float LevelUpScore = 20f;
    public static float LevelScale = 1f;

    public static GameObject Last;


    public bool chasestate = false;       //0 if not chase, 1 if in chase
    //public bool movefar = true;

    public float IncSpAllowed = 0f;

    // Use this for initialization
    void Start () {
        LevelScale = 1f;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Mathf.Floor(PROGRESS.a) == LevelUpScore/* && movefar*/)
        {
            if (DEBUG.DoLOG) Debug.Log("1. Moving Obstacles FAR");
            //Last.transform.position = new Vector3(Last.transform.position.x + (45f * LevelScale), Last.transform.position.y, Last.transform.position.z);
            Last.transform.DOMoveX(Last.transform.position.x + (45f * LevelScale), 0.3f);

            //BRING THE AI
            AIMain.SetActive(true);
            AIMain.GetComponent<MoveAI>().enabled = true;
            AI.transform.parent = null;
            AI.gameObject.transform.position = new Vector3(Respawner.transform.position.x, AI.transform.position.y, Respawner.transform.position.z);
            AI.GetComponent<Chase>().enabled = true;
            AIMain.GetComponent<MoveAI>().incSp = IncSpAllowed;
            StartCoroutine(IncreaseIncSPCount());

            //Move Camera Back
            //StartCoroutine(ChaseCamSetup());


            LevelUpScore += 35f * (IncSpAllowed+1f);
        }
    }

    public static IEnumerator ChaseCamSetup()
    {
        GameObject Cam = Camera.main.gameObject;
        Cam.transform.DOLocalMove(new Vector3(0.1754f, 0.07999999f, -0.1494f),2f);

        yield return null;
    }

    public static IEnumerator ChaseCamOver()
    {
        GameObject Cam = Camera.main.gameObject;
        Cam.transform.DOLocalRotate(new Vector3(7.1066f, -49.5051f, -1.5348f), 1.8f);
        Cam.transform.DOLocalMove(new Vector3(0.1439869f, 0.07999999f, -0.07700403f), 1.8f);

        yield return new WaitForSeconds(0.5f);
        /*GameObject X = GameObject.FindGameObjectWithTag("AI");
        X.SetActive(false);*/
    }

    IEnumerator IncreaseIncSPCount()
    {
        yield return new WaitForSeconds(5f);
        IncSpAllowed += 1f;
    }
}
