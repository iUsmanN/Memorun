using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TheNextFlow.UnityPlugins;

public class Menu : NetworkBehaviour
{
    public LevelUP ControllerHere;

    public Button Camp, Arc, Multi;


    bool AboutBool = false;
    public bool InMenu = false;
    public bool isAIMode = false;

    public GameObject FBLeaderB, AIMain, SettingsObj;

    public GameObject OtherOpts, OtherMyName, FB, HOMEARROW;               //OthermyName is about 

    public GameObject ReplButton, MyScoreTT, HighscoreTT, ScreenshotUI;

    GameObject RainyM, AI;

    public Transform Tut1;
    public NetworkManager manager;
    public AudioClip CampaignMusic, RunningClip;

    public Transform EyeLid, MAINPLAYBUTTON;
    public SpriteRenderer PLAY;

    public Collider SpineCol, PlayerCol;
    public AudioSource /*A,*/ B, PlayerBody;
    public GameObject PlayButton, Score, ScoreEn;
    public Image Dark;
    public Text Title;
    public Camera Cam;
    public Canvas X;
    public move m;
    public GameObject world;
    public ColorSystem cs;
    public GameObject BlackIn;
    public GameObject Player;
    public ColorSystem CS;
    float Tt = 0;
    public Canvas ReplayC;
    public Text Quote;
    public GameObject[] Respawns;

    public GameObject LobbyM;

    public Button BB;
    Transform t, CamDef;
    // Use this for initialization

    Transform[] BuildingsPos;
    Transform PlayerPos;

    public GameObject AIP;

    GameObject MULTIB, EnMultiP;

    IEnumerator FadeBWMusic(AudioSource A, AudioClip a, float duration)
    {
		if(Settings.MusicOn)
		{//yield return new WaitForSeconds(0.5f);
        A.DOPitch(0.5f, 1f);
        A.DOFade(0f, (duration / 2));
        yield return new WaitForSeconds(1f);
        A.clip = a;
        A.Play();
        A.DOPitch(1f, 1f);
        A.DOFade(0.75f, (duration / 2));
		}
    }

    void Awake()
    {
#if UNITY_STANDALONE_WIN
                Display.main.SetRenderingResolution(1920, 788);
                if(DEBUG.DoLOG) Debug.Log("CUSTOM RES SET");
#endif

#if UNITY_ANDROID
        //Display.main.SetRenderingResolution(1920, 788);
        if(DEBUG.DoLOG) Debug.Log("ANDROID");
#endif
    }

    void Start()
    {
        //FB Demo
        //Title.text = QualitySettingsObj.GetQualityLevel().ToString();
        //Camp.interactable = false;
        //Multi.interactable = false;
        //Arc.interactable = false;
        

        //Cam.transform.DOShakeRotation(1f);
        InMenu = false;

        ScreenshotUI.SetActive(false);

        //OtherOpts.transform.localPosition = new Vector3(OtherOpts.transform.position.x, Cam.ScreenToWorldPoint(new Vector3(0, 0)).y - 30f, OtherOpts.transform.position.z);

        if (OtherOpts)
        {
            OtherOpts.gameObject.SetActive(true);
            OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y + 30f * (Screen.height / 479) * 1.2f, 0.05f);
        }

        if (LobbyM)
            LobbyM.SetActive(false);

        MULTIB = GameObject.FindGameObjectWithTag("MULTIPLAYERBUTTONS");
        MULTIB.SetActive(false);

        Title.material.DOFade(1f, 0.1f);

        EyeLid.gameObject.SetActive(true);

        ReplayC.enabled = false;
        BB.gameObject.SetActive(false);
        //Dark.material.DOFade(0f, 0.1f);

        Respawns = GameObject.FindGameObjectsWithTag("Respawn");

        PlayerPos = Player.transform;

        //.material.DOFade(0f, 0.2f);
        //BlackIn.GetComponent<Material>().DOColor(new Color(0f, 0f, 0f, 1000f), 20f);
        CamDef = Cam.gameObject.transform;

        if (GameObject.FindGameObjectWithTag("RainyMemory"))
            Time.timeScale = 0.05f;
        X.enabled = false;
        B.Play();
		if(Settings.MusicOn)
        B.volume = 0.8f;
        m.enabled = false;
        //world.SetActive(false);
        //cs.enabled = false;
        //7.1066
        StartCoroutine(CamPos2());
        //-49.5051
        //-1.5348

        AI = GameObject.FindGameObjectWithTag("AI");
        if (AI)
            AI.SetActive(false);

        EnMultiP = GameObject.FindGameObjectWithTag("EnMultiP");
        EnMultiP.SetActive(false);

        RainyM = GameObject.FindGameObjectWithTag("RainyMemory");
    }

    // Update is called once per frame
    void Update()
    {
        //if(DEBUG.DoLOG) Debug.Log(Screen.height);

        //if(DEBUG.DoLOG) Debug.Log(Time.timeScale);
        //if (Input.getmou)
        //{
            //GlobalPlay();
        //}

        if (Input.GetKeyDown(KeyCode.Escape) && InMenu)
        {
            //Replay();
            MAINPLAYBUTTON.gameObject.SetActive(true);
            StartCoroutine(CamPos2());

            if (OtherOpts)
            {
                OtherOpts.gameObject.SetActive(true);
                //OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y + 7.5f, 0.05f);
                OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y + 30f * (Screen.height / 479) * 1.2f, 0.05f);
            }

        }

        Tt += Time.deltaTime;

        if (Tt % 20f == 0f)
            cs.RandomBuilding();
        //if(DEBUG.DoLOG) Debug.Log("Tt : " + Tt);
    }

    public void OpenBar()
    {
        if (OtherOpts)
        {
            OtherOpts.gameObject.SetActive(true);
            //OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y + 7.5f, 0.05f);
            OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y + 30f * (Screen.height / 479) * 1.2f, 0.05f);
        }
    }

    IEnumerator MUSICSWITCH()
    {
		if (Settings.MusicOn) {
			B.DOFade (0f, 1f);
			yield return new WaitForSeconds (0.8f);

			B.clip = CampaignMusic;
			B.DOFade (1f, 1f);
		}
    }

    public void PressArcadeAI()
    {
        GameObject.FindGameObjectWithTag("Arcade").GetComponent<LevelUP>().enabled = true;
    }

    public void PressArcade()
    {
        InMenu = false;
        /*
        if (PlayerPrefs.GetFloat("CampaignCompleted", 0f) == 0f)
        {
#if UNITY_ANDROID
            AndroidNativePopups.OpenToast("Complete Campaign to unlock Sprint !", AndroidNativePopups.ToastDuration.Short);
#endif
        }
        */

        m.enabled = true;
        //if(DEBUG.DoLOG) Debug.Log("Play pressed!");
        X.enabled = true;
        cs.enabled = true;

        //Fade text
        Title.material.DOFade(0f, 0.3f);

        PLAY.DOFade(0f, 0.3f);

        if(DEBUG.DoLOG) Debug.Log("Load Arcade");
        RainyM.SetActive(false);

        //RenderSettingsObjObj.fogDensity = 0.032f;

        StartCoroutine(SpeedUp(2));

        StartCoroutine(CamPos());

        React.isPlaying = true;
    }

    public void TitlePressed()
    {
        StartCoroutine(CamPosMultiplayer());
        if(DEBUG.DoLOG) Debug.Log("HERE");
    }

    public void PressMultiplayer()
    {
        InMenu = false;
        StartCoroutine(CamPosMultiplayer());

        //EnMultiP.SetActive(true);

        GameObject.FindGameObjectWithTag("MULTIPLAYERBUTTONS").SetActive(true);
        if (GameObject.FindGameObjectWithTag("EnMultiP"))
            manager = GameObject.FindGameObjectWithTag("EnMultiP").GetComponent<NetworkManager>();

        if(DEBUG.DoLOG) Debug.Log(Network.player.ipAddress.ToString());


    }

    public void PressPlay()
    {

        InMenu = false;

        GameObject.FindGameObjectWithTag("Arcade").SetActive(false);

        StartCoroutine(FadeBWMusic(B, CampaignMusic, 1.5f));
        //world.SetActive(true);
        m.enabled = true;
        //if(DEBUG.DoLOG) Debug.Log("Play pressed!");
        X.enabled = true;
        cs.enabled = true;


        //Fade text
        Title.material.DOFade(0f, 0.3f);
        PLAY.DOFade(0f, 0.3f);



        StartCoroutine(SpeedUp(2));

        StartCoroutine(CamPos());

        PlayerBody.clip = RunningClip;
        PlayerBody.Play();

		if(Settings.MusicOn)
        B.DOFade(0.35f, 5f);

        React.isPlaying = true;
    }


    IEnumerator TutorialCR2()
    {
        //Wait
        //SlowDownTime
        //Tell about Jumping
        //SpeedUpTime
        //TutorialComplete
        yield return null;
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.2f);
        BlackIn.GetComponent<Material>().DOFade(0f, 2f);
    }

    public IEnumerator CamPos()
    {
        //StopCoroutine(CamPos2());
        Cam.transform.DOLocalRotate(new Vector3(7.1066f, -49.5051f, -1.5348f), 5f);
        Cam.transform.DOLocalMove(new Vector3(0.1439869f, 0.07999999f, -0.07700403f), 5f);
        yield return new WaitForSeconds(1f);

        //if(GameObject.fin)

        if (OtherOpts)
            OtherOpts.gameObject.SetActive(false);
        if (Score)
            Score.SetActive(true);
        yield return null;
    }
    IEnumerator CamPos2()
    {
        InMenu = false;
        EyeLid.DOLocalMoveY(1.28f, 0.2f);
        Cam.transform.DOLocalRotate(new Vector3(-14.488f, 203.0219f, 0f), .04f);
        Cam.transform.DOLocalMove(new Vector3(0.077f, 0.1468f, 0.0617f), 0.04f);
        //Title.transform.DOLocalMoveX(Title.transform.position.x + 10f, 0.5f);
        yield return new WaitForSeconds(0.2f);

        EyeLid.gameObject.SetActive(false);
    }

    IEnumerator CamPosMultiplayer()
    {
        MAINPLAYBUTTON.gameObject.SetActive(false);
        MULTIB.SetActive(true);
        //Cam.transform.DORotate(new Vector3(-54.164f, -156.973f, 0f), 0.15f);
        Cam.transform.parent = null;

        Cam.transform.DOLocalRotate(new Vector3(-0.83f, -66.973f, 0f), 0.03f);
        Cam.transform.DOLocalMove(new Vector3(-79.9f, 13.7f, -2.5f), 0.03f);

        //this.transform.parent = null;

        yield return new WaitForSeconds(0.5f);

        //if (OtherOpts)
        //    OtherOpts.gameObject.SetActive(false);
    }

    IEnumerator CamPosAbout()
    {
        //MAINPLAYBUTTON.gameObject.SetActive(false);
        //MULTIB.SetActive(true);
        //Cam.transform.DORotate(new Vector3(-54.164f, -156.973f, 0f), 0.15f);

        Cam.transform.parent = null;
        Cam.transform.DOLocalRotate(new Vector3(-0.83f, -66.973f, 0f), 0.03f);
        Cam.transform.DOLocalMove(new Vector3(-79.9f, 13.7f, -2.5f), 0.02f);
        yield return null;
        //if (OtherOpts)
        //    OtherOpts.gameObject.SetActive(false);
    }

    public void DownArrowPressed()
    {
        GameObject.FindGameObjectWithTag("MULTIPLAYERBUTTONS").SetActive(false);
        RainyM.SetActive(true);
        Camera.main.transform.parent = Player.transform;
        MainPlayPressed();
        EnMultiP.SetActive(false);
    }

    IEnumerator CamPos3GameModeMenu()
    {
        StopCoroutine(CamPos2());
        //yield return new WaitForSecondsRealtime(1f);
        //Cam.transform.DOLocalRotate(new Vector3(-6.782001f, -216.78f, -0.404f), 0.1f);
        Cam.transform.DOLocalRotate(new Vector3(-17.459f, -156.974f, 0f), 0.03f);
        Cam.transform.DOLocalMove(new Vector3(0.085f, 0.0119f, 0.065f), 0.03f);
        //Title.transform.DOLocalMoveX(Title.transform.position.x + 10f, 0.5f);
        yield return new WaitForSeconds(0.8f);
        MULTIB.SetActive(false);

        if (OtherOpts)
        {
            OtherOpts.gameObject.SetActive(true);
            //OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y -30f, 0.01f);
            OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y - 30f * (Screen.height / 479) * 1.2f, 0.05f);
        }
        //MAINPLAYBUTTON.gameObject.SetActive(false);
    }

    public void CloseBar()
    {
        if (OtherOpts)
        {
            OtherOpts.gameObject.SetActive(true);
            //OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y -30f, 0.01f);
            OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y - 30f * (Screen.height / 479) * 1.2f, 0.07f);
        }
    }

    IEnumerator StopA()
    {
        yield return new WaitForSeconds(2f);
        //A.Stop();
    }

    public IEnumerator SpeedUp(int t)
    {
        while (Time.timeScale < 1.2f)
        {
            //if(DEBUG.DoLOG) Debug.Log(i);
            yield return new WaitForSeconds(0.003f);
            Time.timeScale += 0.013f;
        }
        ReplayC.enabled = true;
    }

    public void About()
    {
        if(DEBUG.DoLOG) Debug.Log("About Pressed!");
    }

    public void MainPlayPressed()
    {

        StartCoroutine(CamPos3GameModeMenu());
        EyeLid.gameObject.SetActive(false);
        if (OtherOpts && !InMenu)
        {
            OtherOpts.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y - 30f * (Screen.height / 479) * 1.2f, 0.01f);
        }

        InMenu = true;
    }

    public void Replay()
    {
        cs.HideBC();
        BB.gameObject.SetActive(false);
        StartCoroutine(ReplayCour());
        PROGRESS.a = 0f;
        //SelectRandomQuote();
        /*
        //Player.transform.position = new Vector3(-68.57f, Player.transform.position.y, Player.transform.position.z);
        Player.transform.DOMoveX(-68.57f, 1f);
        Player.GetComponent<Animator>().SetBool("die", false);
        Player.GetComponent<Animator>().SetBool("dieup", false);

        //Dark.material.DOFade(0f, 0.1f);

        //StopAllCoroutines();
        StopCoroutine(LookBuildingExpand());
        StopCoroutine(ThroughTheTrees());
        StopCoroutine(EndingTheTrees());
        StopCoroutine(ZoomOut());
        //if(DEBUG.DoLOG) Debug.Log("HERE");
        
        StopAllCoroutines();
        StartCoroutine(SpeedUp(2));
        StopCoroutine(CS.Colors());
        Cam.DOFieldOfView(60, 2f);
        m.Sp = 8f;
        B.Play();

        cs.enabled = false;
        cs.enabled = true;

        //StopCoroutine(CS.Colors());
        //StartCoroutine(CS.Colors());

        B.pitch = 1f;
        StartCoroutine(CamPos());

        Respawning();*/

        PlayerBody.clip = RunningClip;
        PlayerBody.loop = true;
        PlayerBody.Play();
    }

    IEnumerator ReplayCour()
    {
        B.DOPitch(1f, 1.5f);
     
		if(Settings.MusicOn)
		B.DOFade(1f, 1.2f);

        //if (AIP)
        //    AIP.transform.DOScale(new Vector3(0f, 0f, 0f), 0.5f);
        //Respawning();

        //StopAllCoroutines();
        Quote.material.DOFade(0f, 0.1f);
        Title.material.DOFade(0f, 0.3f);
        yield return new WaitForSeconds(0.2f);
        Title.material.color = new Color(1f, 1f, 1f, 0f);
        ReplayC.enabled = false;

        StartCoroutine(SpeedUp(2));

        SpineCol.enabled = false;
        if (GameObject.FindGameObjectWithTag("RainyMemory"))
        {
            Player.transform.DOMoveX(-68.57f, 0.75f);
            m.Sp = 8f;
            StartCoroutine(CamPos());
        }

        else if (GameObject.FindGameObjectWithTag("Arcade"))
        {
            Player.transform.DOMoveX(Player.transform.position.x - 50f, 0.75f);
            ScoreEn.transform.position = new Vector3(Player.transform.position.x - 40f, GameObject.FindGameObjectWithTag("Change Color").transform.position.y, GameObject.FindGameObjectWithTag("Change Color").transform.position.z);
            GameObject.FindGameObjectWithTag("ArcadeMan").transform.DOMoveX(GameObject.FindGameObjectWithTag("ArcadeMan").transform.position.x + 40f, 1.5f);

            if (AIP && isAIMode)
            {
                if(DEBUG.DoLOG) Debug.LogError("Replay AI");

                AIP.transform.DOMoveX(Player.transform.position.x - 50f, 0.75f);
            }

            else if (AIP && !isAIMode)
            {
                if(DEBUG.DoLOG) Debug.LogError("Replay Arcade");

                AIP.transform.DOMoveX(-100f, 0.75f);
                AIP.GetComponent<Chase>().enabled = false;
                AIMain.SetActive(true);
                AIMain.GetComponent<MoveAI>().enabled = true;

                AIMain.GetComponent<MoveAI>().incSp = 0f;
                ControllerHere.IncSpAllowed = 0f;

                AIP.GetComponent<Animator>().SetBool("Catch", false);
                GameObject.FindGameObjectWithTag("Arcade").GetComponent<LevelUP>().LevelUpScore = 30f;
            }

            m.Sp = 8f;
            StartCoroutine(CamPos());
        }
        /*
        if (!GameObject.FindGameObjectWithTag("RainyMemory"))
        {
            if (AIP && isAIMode)
            {
                if(DEBUG.DoLOG) Debug.LogError("Replay AI");

                AIP.transform.DOMoveX(Player.transform.position.x - 50f, 0.75f);

                m.Sp = 8f;
                StartCoroutine(CamPos());
            }

            else if (AIP && !isAIMode)
            {
                if(DEBUG.DoLOG) Debug.LogError("Replay Arcade");

                AIP.transform.DOMoveX(-100f, 0.75f);
                AIP.GetComponent<Chase>().enabled = false;
                AIMain.SetActive(true);
                AIMain.GetComponent<MoveAI>().enabled = true;
                GameObject.FindGameObjectWithTag("Arcade").GetComponent<LevelUP>().LevelUpScore = 50f;

                m.Sp = 8f;
                StartCoroutine(CamPos());
            }
        }
        */
        //yield return new WaitForSeconds(0.4f);

        Player.GetComponent<Animator>().SetBool("die", false);
        Player.GetComponent<Animator>().SetBool("dieup", false);

        yield return new WaitForSeconds(1f);
        //if (AIP)
        //AIP.transform.DOScale(new Vector3(1f, 1f, 1f), 0.7f);
        //SpineCol.enabled = true;
        //if(DEBUG.DoLOG) Debug.Log("Spine En");
        Title.material.color = new Color(1f, 1f, 1f, 0f);

        //Dark.material.DOFade(0f, 0.1f);

        //StopAllCoroutines();
        //if(DEBUG.DoLOG) Debug.Log("HERE");

        StopAllCoroutines();
        //        StartCoroutine(SpeedUp(2));
        StopCoroutine(CS.Colors());
        Cam.DOFieldOfView(60, 2f);
        m.Sp = 8f;
        //B.Play();

        cs.enabled = false;
        cs.enabled = true;

        //StopCoroutine(CS.Colors());
        //StartCoroutine(CS.Colors());

        //DOTween.Clear();

        StartCoroutine(CamPos());
        //StartCoroutine(CS.Colors());

        Title.material.color = new Color(1f, 1f, 1f, 0f);

        PlayerCol.enabled = true;

        Respawning();

        Time.timeScale = 1.2f;
    }

    void Respawning()
    {
        if (Respawns.Length > 0)
        {
            //Respawn
            foreach (GameObject g in Respawns)
            {
                if (g.GetComponent<RespawnControl>())
                    g.GetComponent<RespawnControl>().RespawnNow();
                if (g.GetComponent<Animator>())
                {
                    g.GetComponent<Animator>().Rebind();
                }
            }
            Title.material.color = new Color(1f, 1f, 1f, 0f);
            Time.timeScale = 1.2f;
        }
    }

    public void ClickHost()
    {
        Application.LoadLevel(0);

        /*
         //Destroy(GameObject.FindGameObjectWithTag("OriginalMainPlayer"));
         GameObject.FindGameObjectWithTag("OriginalMainPlayer").SetActive(false);
         RainyM.SetActive(false);

         //MultiPlayer
         GameObject.FindGameObjectWithTag("EnMultiP").GetComponent<NetworkManager>().StartHost();
         */
    }

    public void ClickJoin()
    {
        /*
#if UNITY_ANDROID
        AndroidNativePopups.OpenToast("Still Developing. Sabr.", AndroidNativePopups.ToastDuration.Long);
#endif
        */
        /*
        GameObject.FindGameObjectWithTag("OriginalMainPlayer").SetActive(false);
        RainyM.SetActive(false);

        //MultiPlayer
        //LAN
        manager.StartClient();

        //Internet
        //manager.StartMatchMaker();
        //manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate, manager.OnMatchCreate, 1,
        */
    }

    IEnumerator AIResetStumble()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            Player.GetComponent<Animator>().SetBool("stumhigh", false);
        }
    }

    public void ClickAI()
    {
        InMenu = false;

        //        if (PlayerPrefs.GetFloat("CampaignCompleted", 0f) == 0f)
        //        {
        //#if UNITY_ANDROID
        //            AndroidNativePopups.OpenToast("Complete Campaign to unlock Sprint !", AndroidNativePopups.ToastDuration.Short);
        //#endif
        //        }

        isAIMode = true;
        AI.SetActive(true);
        Camera.main.transform.parent = Player.transform;
        PressArcade();

        React.isPlaying = true;
        PROGRESS.ai = true;
        PROGRESS.a = 0f;

        AIP.transform.parent = null;
        //StartCoroutine(AIResetStumble());
    }

    public void ClickShareScreenShot()
    {
        StartCoroutine(ClickShareScoreCR());
    }

    IEnumerator ClickShareScoreCR()
    {


        ScreenshotUI.SetActive(true);

        ScreenshotUI.transform.Find("Text").GetComponent<Text>().text = MyScoreTT.GetComponent<Text>().text;

        if (GameObject.FindGameObjectWithTag("RainyMemory"))
            ScreenshotUI.transform.Find("Text (1)").GetComponent<Text>().text = "percent\ncampaign  mode";

        else if (GameObject.FindGameObjectWithTag("Arcade") && !isAIMode)
            ScreenshotUI.transform.Find("Text (1)").GetComponent<Text>().text = "      in  arcade  mode";

        else if (GameObject.FindGameObjectWithTag("Arcade") && isAIMode)
            ScreenshotUI.transform.Find("Text (1)").GetComponent<Text>().text = "      versus  ai";

        ReplButton.SetActive(false);
        MyScoreTT.GetComponent<Text>().enabled = false;
        HighscoreTT.SetActive(false);
        yield return new WaitForSeconds(0.07f);
        ScreenshotUI.SetActive(false);
        MyScoreTT.GetComponent<Text>().enabled = true;
        HighscoreTT.SetActive(true);
        ReplButton.SetActive(true);
    }

    public void ClickAbout()
    {
        CloseBar();
        OtherMyName.SetActive(true);
        StartCoroutine(CamPosAbout());
        /*
        if (!AboutBool)
        {
            OtherMyName.SetActive(true);
            StartCoroutine(CamPosAbout());
        }

        else if (AboutBool)
        {
            OtherMyName.SetActive(false);
            Cam.transform.parent = Player.transform;
            StartCoroutine(CamPos2());
        }
        */
        AboutBool = !AboutBool;
    }

    public void OpenAbout()
    {
        OtherMyName.transform.DOScale(new Vector3(1f, 1f, 1f), 0.03f);
    }

    public void ClickFB()
    {
        CloseBar();

        if (FB)
            FB.SetActive(true);

        StartCoroutine(CamPosAbout());
    }

    public void ClickHomeArrow()
    {
        OpenBar();

        /*
        if (GameObject.FindGameObjectWithTag("About"))
            GameObject.FindGameObjectWithTag("About").SetActive(false);

        if (GameObject.FindGameObjectWithTag("FB"))
            GameObject.FindGameObjectWithTag("FB").SetActive(false);

        */
        StartCoroutine(HOMEARR());

        Cam.transform.parent = Player.transform;
        StartCoroutine(CamPos2());
    }

    IEnumerator HOMEARR()
    {

        FBLeaderB.transform.DOScaleY(0f, 0.03f);
        OtherMyName.transform.DOScaleY(0f, 0.03f);
        SettingsObj.transform.DOScaleY(0f, 0.02f);

        yield return new WaitForSeconds(0.05f);
        FBLeaderB.SetActive(false);
        OtherMyName.SetActive(false);
        yield return null;
    }

    public void OpenHomeArr()
    {
        HOMEARROW.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y * (Screen.height / 479) * 1.3f, 0.04f);
    }

    public void CloseHomeArr()
    {
        HOMEARROW.transform.DOMoveY(Cam.ScreenToWorldPoint(new Vector3(0, 0)).y - 50f * (Screen.height / 479) * 1.2f, 0.04f);
    }

    public void LinkedInPressed()
    {
        Application.OpenURL("https://linkedin.com/in/muhammad-usman-nazir-1b84a0106/");
    }

    public void TwitterPressed()
    {
        Application.OpenURL("https://twitter.com/iUsmanN");
    }

    public void SettingsObjObjPressed()
    {
        StartCoroutine(CamPosAbout());
        SettingsObj.SetActive(true);
        SettingsObj.transform.DOScale(new Vector3(1f, 1f, 1f), 0.03f);
    }

    public void ReplaySpeed()
    {
        StartCoroutine(RepSp());
    }

    IEnumerator RepSp()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1.2f; if(DEBUG.DoLOG) Debug.Log("Speed Set");
    }

    public void OpenFBUI()
    {
        FBLeaderB.transform.DOScaleY(1f, 0.03f);
    }
}
