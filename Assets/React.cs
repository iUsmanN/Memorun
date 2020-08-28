using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using Facebook.Unity;
using TheNextFlow.UnityPlugins;

public class React : MonoBehaviour
{
    public GameObject TapToQuit;
    public FBScript FBSCRIPT;

    public GameObject HIGHSCORE, ScoreText;

    public AudioClip Kheyal, Die;

    public GameObject RT, Menu;

    public Animator Jumper, Shooter, DyingPerson, DyingPerson2;

    public Transform EyeLid, FallenMan, Tut1, Tut2;

    bool CanReplay;

    public Animator Anim;
    public Collider Col;
    public move MOVE;
    public Camera Cam;
    public GameObject spine, PauseC;
    public AudioSource Audio, PlayerBody;
    public ColorSystem CS;

    public Button B;
    public Text ReplayText;
    public Text Quote;
    public Canvas ReplayC;

    string[] texts;

    public static bool isPaused = false;
    public static bool isPlaying = false;

    float i, j;
    void SelectRandomQuote()
    {
        //i = -1f;
        j = Random.Range(0, 20);
        while (j == i)
        {
            j = Random.Range(0, 20);
        }

        if (Quote)
            Quote.text = texts[(int)j];
        if (DEBUG.DoLOG) Debug.Log("Selected Random Quote1");
        i = j;
    }

    void SetTexts()
    {
        texts = new string[20];
        texts[0] = "\" There's only one thing  more precious than time and that's who we spend it on. \"\n                                            - Leo Christopher";
        texts[1] = "\"Our memory is a more perfect world than the universe: it gives back life to those who no longer exist.\"\n                                   - Guy de Maupassant";
        texts[2] = "\"What you seek is seeking you.\" \n                                      -Rumi";
        texts[3] = "\"Why are you so enchanted by this world, when a mine of gold lies within you ?\"\n                                              -Rumi";
        texts[4] = "\"The past is already gone, the future is not yet here. There's only one moment for you to live.\" \n                                        -Buddha";
        texts[5] = "\"Patience ensures victory.\"\n                                        -Imam Ali";
        texts[6] = "\"Life's too mysterious to take too serious.\"\n                                   -Mary Engelbreit";
        texts[7] = "\"There isn't a way things should be. There's just what happens, and what we do.\"\n                                       -Terry Pratchett";
        texts[8] = "\"Trying again is the key of glorious victory\"\n                                           -Iqbal";
        texts[9] = "\"Believe you can and you're halfway there.\"\n                                     -T.Roosevelt";

        texts[10] = "\"In order to succeed you must fail, so that you know what not to do the next time.\"\n                                        - Anthony J.D'Angelo";
        texts[11] = "\"Don't grieve. Anything you lose comes round in another form.\"\n                                         -Rumi";
        texts[12] = "\"Failure is a word unknown to me.\"\n                                       -Jinnah";
        texts[13] = "\"Failure is not fatal until we surrender.\"\n                                        -Iqbal";
        texts[14] = "\"The cure for ignorance is to question.\"\n                                  -Prophet Muhammad";

        texts[15] = "\"Be in this world like a stranger or one who is passing through.\"\n                                        -Prophet Muhammad";
        texts[16] = "\"No matter how difficult the past, you can always begin again.\"\n                                          -Buddha";
        texts[17] = "\"To be ready to fail is to be prepared for success.\"\n                                       -Jose Bergamin";
        texts[18] = "\"You've Got To Be Willing To Lose Everything To Gain Yourself.\"\n                                        -Iyanla Vanzant";
        texts[19] = "\"A man's measure is his will.\"\n                                     -Imam Ali";
    }

    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.SetInt("Tutorial", 0);
        //PlayerPrefs.SetInt("Tutorial", 0);
        isPaused = false;

        if (PlayerPrefs.GetInt("Tutorial") >= 2)
        {
            if(DEBUG.DoLOG) Debug.Log("DELTUT");
            Tut1.gameObject.SetActive(false);
            Tut2.gameObject.SetActive(false);
        }

        if (!Anim)
            Anim = GetComponent<Animator>();

        GameObject.FindGameObjectWithTag("Lean").GetComponent<Lean.Touch.LeanSwipeDirection4>().Anim = Anim;
        GameObject.FindGameObjectWithTag("Lean").GetComponent<Lean.Touch.LeanSwipeDirection4>().ReactS = this;


        isPlaying = false;
        PauseC = GameObject.FindGameObjectWithTag("Pause");
        if (PauseC)
            PauseC.SetActive(false);

        i = -1f;
        //Quote.material.DOFade(0f, 0.1f);
        SetTexts();

        CanReplay = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("isplaying : " + isPlaying);
        //Debug.Log("Tutorial : " + PlayerPrefs.GetInt("Tutorial"));

        if (Input.GetKeyDown(KeyCode.Escape) && (Anim.GetBool("die") || Anim.GetBool("dieup")) && isPlaying)
        {
            Debug.LogWarning("HOME");
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !Anim.GetBool("die") && !Anim.GetBool("dieup") && isPlaying)
        {
            if (!isPaused)
                PauseHandling();

            else
                SceneManager.LoadScene(1);
        }

        if (Input.GetMouseButtonDown(0) && CanReplay)
            StartCoroutine(REPLAYLEVEL());

    }
    public void PauseHandling()
    {
        if (DEBUG.DoLOG) Debug.Log("Pause toggle");
        if (!isPaused)
        {
            PlayerBody.volume = 0f;
            //Audio.pitch = 0.9f;
            Time.timeScale = 0.0f;
            PauseC.SetActive(true);
            isPaused = !isPaused;
        }

        else if (isPaused)
        {
            PlayerBody.volume = 1f;
            Audio.pitch = 1f;
            Time.timeScale = 1.2f;
            PauseC.SetActive(false);
            isPaused = !isPaused;
        }
    }

    IEnumerator LAST()
    {

        TapToQuit.SetActive(false);

        Quote.material.DOFade(0f, 0.2f);

        EyeLid.gameObject.SetActive(true);
        EyeLid.DOLocalMoveY(0.81f, 0.1f);

        yield return new WaitForSeconds(1f);

        Cam.transform.parent = null;
        //Cam.transform.position = new Vector3(600f, -18.7f, 36.9f);
        //Cam.transform.rotation = new Quaternion(90f, 0f, 253f, 0f);
        Cam.transform.DORotate(new Vector3(90f, 0f, 253f), 0.1f);
        Cam.transform.position = FallenMan.transform.position;


        Cam.transform.DOLocalMoveY(85f, 80f);
        Cam.transform.DOLocalMoveX(300f, 200f);
        Cam.transform.DOLocalMoveZ(-20f, 200f);

        Cam.transform.Rotate(new Vector3(0f, 20f * Time.deltaTime, 0f));

        //Cam.transform.position = new Vector3(0.007f, 0.0107f, -0.0621f);
        EyeLid.DOLocalMoveY(1.24f, 2f);

        Quote.text = "\"And then it flows through me like rain, and I can't feel anything but gratitude...\n For every single moment, of this stupid little life...\"\n                                  -Unknown";

        StartCoroutine(ShowButtonsAndQuote2());
    }

    void Tutorial()
    {
        if (PlayerPrefs.GetInt("Tutorial") <= 2)
        {
            if (DEBUG.DoLOG) Debug.Log("Give tutorial");
        }
    }

    public IEnumerator TutorialCR1()
    {
        if (PlayerPrefs.GetInt("Tutorial") < 1)
        {
            //PlayerPrefs.SetInt("Tutorial", 1);
            if (DEBUG.DoLOG) Debug.Log("Give tutorial1");
            //SlowDownTime
            Time.timeScale = 0.2f;
            //Tell about crouching
            Tut1.DOScaleY(0.01529f, 0.1f);
        }
        yield return null;
    }

    public IEnumerator TutorialCR2()
    {
        if (PlayerPrefs.GetInt("Tutorial") < 2)
        {
            //PlayerPrefs.SetInt("Tutorial", 2);
            if (DEBUG.DoLOG) Debug.Log("Give tutorial2");
            //SlowDownTime
            Time.timeScale = 0.2f;
            //Tell about crouching
            Tut2.DOScaleY(0.01529f, 0.1f);
        }
        yield return null;
    }

    void OnTriggerStay(Collider Col)
    {
        if (Col.gameObject.tag == "Tut1")
        {
            if (Anim.GetBool("die") && Anim.GetBool("dieup"))
                Tut1.transform.localScale = new Vector3(Tut1.transform.localScale.x, 0f, Tut1.transform.localScale.z);
        }

        if (Col.gameObject.tag == "Tut2")
        {
            if (Anim.GetBool("die") && Anim.GetBool("dieup"))
                Tut2.transform.localScale = new Vector3(Tut2.transform.localScale.x, 0f, Tut2.transform.localScale.z);
        }
    }

    void OnTriggerExit(Collider Col)
    {
        if (Col.gameObject.tag == "Tut1" && PlayerPrefs.GetInt("Tutorial") < 2)
        {
            Tut1.transform.localScale = new Vector3(Tut1.transform.localScale.x, 0f, Tut1.transform.localScale.z);
            PlayerPrefs.SetInt("Tutorial", 1);
            Time.timeScale = 1.2f;
            //Quote.material.DOFade(0f, 0.2f);
        }

        if (Col.gameObject.tag == "Tut2" && PlayerPrefs.GetInt("Tutorial") < 3)
        {
            Tut2.transform.localScale = new Vector3(Tut2.transform.localScale.x, 0f, Tut2.transform.localScale.z);
            PlayerPrefs.SetInt("Tutorial", 2);
            Time.timeScale = 1.2f;
            //Quote.material.DOFade(0f, 0.2f);
        }

        if (Col.gameObject.tag == "LowCube" || Col.gameObject.tag == "HighCube")
        {
            StartCoroutine(DisableSpJump());
        }
    }

    IEnumerator DisableSpJump()
    {
        yield return new WaitForSeconds(0.2f);
        Anim.SetBool("SpJumpEn", false);
    }

    void OnTriggerEnter(Collider Col)
    {
        if (DEBUG.DoLOG) Debug.Log("Here");

        if (Col.gameObject.tag == "LowCube" || Col.gameObject.tag == "HighCube")
        {
            Anim.SetBool("SpJumpEn", true);
        }

        if (Col.gameObject.tag == "Tut1" && (PlayerPrefs.GetInt("Tutorial") < 1))
        {
            Debug.Log("Give tutorial");
            StartCoroutine(TutorialCR1());
        }

        if (Col.gameObject.tag == "Tut2" && (PlayerPrefs.GetInt("Tutorial") < 2))
        {
            Debug.Log("Give tutorial");
            StartCoroutine(TutorialCR2());
        }


        if (Col.gameObject.tag == "LastScene")
        {
            Audio.DOFade(0f, 3f);
            StartCoroutine(LAST());
        }

        if (Col.gameObject.tag == "EndCampaign")
        {
            //CS.NewColor();
            Cam.transform.DOLocalMove(new Vector3(0.0561f, 0.0393f, -0.0168f), 7f);
            Quote.material.DOFade(0f, 1f);
        }

        if (Col.gameObject.tag == "LastLook")
        {
            //CS.NewColor();
            //Time.timeScale = Mathf.Lerp(Time.timeScale, Time.timeScale * 0.7f, 3f);
            Cam.transform.DOLocalRotate(new Vector3(11.69f, -22.517f, -1.5348f), 11f);
            Cam.transform.DOLocalMove(new Vector3(0.301f, 0.338f, -0.573f), 11f);
        }

        if (Col.gameObject.tag == "LookUp")
        {
            //CS.NewColor();
            //Time.timeScale = Mathf.Lerp(Time.timeScale, Time.timeScale * 0.7f, 3f);
            Cam.transform.DOLocalRotate(new Vector3(339.7013f, 294.3711f, 358.3761f), 5f);
            Cam.transform.DOLocalMoveY(0.025f, 5f);
        }

        if (Col.gameObject.tag == "MainCamera")
        {
            //Time.timeScale = Mathf.Lerp(Time.timeScale, Time.timeScale * 0.7f, 3f);
            Cam.transform.DOLocalRotate(new Vector3(7.1066f, -49.5051f, -1.5348f), 6.5f);
            Cam.transform.DOLocalMove(new Vector3(0.1439869f, 0.07999999f, -0.07700403f), 6.5f);
        }

        if (Col.gameObject.tag == "LookAtShooterHuge")
        {
            //CS.NewColor();
            Cam.transform.DOLocalRotate(new Vector3(-24.709f, -122f, 0f), 10f);
            Cam.transform.DOLocalMove(new Vector3(0.3779981f, 0.022f, 0.06400036f), 7f);
            Debug.Log("K");
        }

        if (Col.gameObject.tag == "LookAtFallingGuy")
        {
            Cam.transform.DOLocalRotate(new Vector3(7.1066f, -49.5051f, -1.5348f), 11f);
            Cam.transform.DOLocalMove(new Vector3(0.1439869f, 0.07999999f, -0.07700403f), 11f);
            Debug.Log("K");
        }

        if (Col.gameObject.tag == "ColEnable")
        {
            //CS.NewColor();
            Col.enabled = true;
            Debug.Log("Spine Collider Enabled");
        }

        if (Col.gameObject.tag == "StartJumpingAnim")
        {
            Jumper.Rebind();
        }

        if (Col.gameObject.tag == "StartKillingAnim")
        {
            Shooter.Rebind();
            DyingPerson.Rebind();
        }

        if (Col.gameObject.tag == "LastLookKill")
        {
            DyingPerson2.Rebind();
        }

        if (Col.gameObject.tag == "END DEMO")
        {
            Debug.Log("ENDING DEMO");
            Screen.SetResolution(16, 9, true, 10);

        }
    }

    /*
    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }

        */

    public void ChaseCatch()
    {
        ShowScore();
        //
        if (Quote)
            Quote.material.DOFade(0f, 0.2f);

        SelectRandomQuote();
        //Debug.Log("DIE");
        //Col.enabled = false;
        if (MOVE)
            MOVE.Sp = 0f;
        Anim.SetBool("die", true);
        Time.timeScale = 0.25f;
        //Cam.DOFieldOfView(16f, 1.2f);
        //Cam.transform.LookAt(spine.gameObject.transform);
        Cam.transform.DOLocalRotate(new Vector3(350f, 300.5509f, 0.3776277f), 1.5f);
        //Cam.transform.LookAt(spine.GetComponent<Transform>());
        Cam.transform.DOLocalMove(new Vector3(0.09f, 0.022f, 0.1982f), 1.5f);
        //Cam.transform.DOLocalMoveY(0.03f, 1.5f);
        //Audio.DOPitch(0f, 0.2f);
        //Cam.DOColor(new Color(0.2f, 0.2f, 0.2f), 1f);
        //StopCoroutine(CS.Colors());
        StartCoroutine(SetDieUp());
        Anim.SetBool("dieup", false);

        PlayerBody.clip = Die;
        PlayerBody.Play();

        StartCoroutine(ShowButtonsAndQuote());
        GetComponent<Collider>().enabled = false;
    }

    public void OnCollisionEnter(Collision A)
    {

        foreach (ContactPoint contact in A.contacts)
        {
            if (DEBUG.DoLOG) print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        //Internet Player Collisions
        if (GameObject.FindGameObjectWithTag("EnMultiP") && !GameObject.FindGameObjectWithTag("AI"))
        {
            Debug.Log("HIT");
        }

        //Single Player
        else
        {
            if (!Menu.GetComponent<Menu>().isAIMode)
            {
                if (Quote)
                    Quote.material.DOFade(0f, 0.2f);
                if (A.gameObject.tag == "LowCube" && (!Anim.GetBool("die") || !Anim.GetBool("dieup")))
                {
                    //ShowScore
                    ShowScore();
                    //
                    if (Quote)
                        Quote.material.DOFade(0f, 0.2f);

                    SelectRandomQuote();
                    //Debug.Log("DIE");
                    //Col.enabled = false;
                    if (MOVE)
                        MOVE.Sp = 0f;
                    Anim.SetBool("die", true);
                    Time.timeScale = 0.25f;
                    //Cam.DOFieldOfView(16f, 1.2f);
                    //Cam.transform.LookAt(spine.gameObject.transform);
                    Cam.transform.DOLocalRotate(new Vector3(350f, -50.9f, 0.3776277f), 1.5f);
                    //Cam.transform.LookAt(spine.GetComponent<Transform>());
                    Cam.transform.DOLocalMove(new Vector3(0.09f, 0.022f, 0.227f), 1.5f);
                    //Cam.transform.DOLocalMoveY(0.03f, 1.5f);
                    //Audio.DOPitch(0f, 0.2f);
                    //Cam.DOColor(new Color(0.2f, 0.2f, 0.2f), 1f);
                    //StopCoroutine(CS.Colors());
                    StartCoroutine(SetDieUp());
                    Anim.SetBool("dieup", false);

                    PlayerBody.clip = Die;
                    PlayerBody.Play();
                    //PlayerBody.loop = false;

                }

                else if (A.gameObject.tag == "HighCube" && (!Anim.GetBool("die") || !Anim.GetBool("dieup")))
                {
                    if (Quote)
                        Quote.material.DOFade(0f, 0.2f);

                    //ShowScore
                    ShowScore();

                    SelectRandomQuote();
                    if (DEBUG.DoLOG) Debug.Log("DIE");
                    //Col.enabled = false;
                    if (MOVE)
                        MOVE.Sp = 0f;
                    Anim.SetBool("dieup", true);
                    Time.timeScale = 0.25f;
                    //Cam.DOFieldOfView(12f, 2f);
                    //Cam.transform.LookAt(spine.gameObject.transform);

                    Cam.transform.DOLocalRotate(new Vector3(353.053f, 329.9439f, 0.1299268f), 1.5f);
                    Cam.transform.DOLocalMove(new Vector3(0.1522003f, 0.0796f, 0.02070001f), 1.5f);

                    /*Cam.transform.DOLocalRotate(new Vector3(343.8942f, 306.1281f, 12.89883f), 2f);
                    Cam.transform.DOLocalMove(new Vector3(0.1885f, 0.01880002f, -0.0921f), 1.5f);*/
                    //Audio.DOPitch(0f, 0.2f);

                    PlayerBody.clip = Die;
                    PlayerBody.Play();
                }

                StartCoroutine(ShowButtonsAndQuote());
                GetComponent<Collider>().enabled = false;

            }

            else if(Menu.GetComponent<Menu>().isAIMode)
            {
                MOVE.Sp = MOVE.Sp * 0.5f;
                if (A.gameObject.tag == "HighCube" && (!Anim.GetBool("die") || !Anim.GetBool("dieup")))
                {
                    Debug.LogError("Stumble");
                    Anim.SetBool("stumup", true);
                }

                else if (A.gameObject.tag == "LowCube" && (!Anim.GetBool("die") || !Anim.GetBool("dieup")))
                {
                    Debug.LogError("Stumble");
                    Anim.SetBool("stumdown", true);
                }
            }
        }
    }

    void OnCollisionExit(Collision A)
    {
        if (Menu.GetComponent<Menu>().isAIMode)
        {
            MOVE.Sp = MOVE.Sp * 2f;
            if (A.gameObject.tag == "HighCube" && (!Anim.GetBool("die") || !Anim.GetBool("dieup")))
            {
                Debug.LogError("Stumble");
                Anim.SetBool("stumup", false);
            }

            else if (A.gameObject.tag == "LowCube" && (!Anim.GetBool("die") || !Anim.GetBool("dieup")))
            {
                Debug.LogError("Stumble");
                Anim.SetBool("stumdown", false);
            }
        }
    }

    IEnumerator ResetStumble()
    {
        yield return new WaitForSeconds(0.5f);
        Anim.SetBool("stumup", false);
        Anim.SetBool("stumdown", false);
    }

    IEnumerator ShowButtonsAndQuote()
    {

        yield return new WaitForSeconds(0.1f);
        Audio.DOPitch(0.35f, 0.9f);
        Audio.DOFade(0.4f, 0.8f);


        //Audio.do
        //yield return new WaitForSeconds(1f);
        //Audio.DOPitch(1f, 0.8f);



        ReplayC.enabled = true;

        yield return new WaitForSeconds(0.3f);
        RT.SetActive(true);
        Menu.SetActive(true);
        if (DEBUG.DoLOG) Debug.Log("O");
        if (B)
            B.enabled = false;

        //Debug.Log("C : " + Quote.material.color.a);

        Quote.material.DOFade(0f, 0.1f);
        //Debug.Log("A : " + Quote.material.color.a);
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("B : " + Quote.material.color.a);
        Quote.material.DOFade(1f, 0.3f);
        if (B)
        {
            B.gameObject.SetActive(true);
            B.enabled = true;
        }
        //Color.Lerp(Quote.color, new Color(1f, 1f, 1f, 1f), 1f);
        //ReplayText.material.DOFade(1f, 0.5f);
        yield return null;
    }

    IEnumerator ShowButtonsAndQuote2()
    {
        yield return new WaitForSeconds(0.5f);
        Audio.clip = Kheyal;
        Audio.DOFade(1f, 5f);
        Audio.PlayDelayed(1f);
        yield return new WaitForSeconds(0.5f);

        ScoreText.SetActive(false);
        HIGHSCORE.SetActive(false);
        PlayerBody.volume = 0f;

        ReplayC.enabled = true;

        RT.SetActive(true);
        Menu.SetActive(true);
        Debug.Log("O");
        if (B)
            B.enabled = false;
        Quote.material.DOFade(1f, 15f);


        B.gameObject.SetActive(true);
        B.GetComponentInChildren<Text>().text = "Campaign   Over.";

        PlayerPrefs.SetFloat("Ended", 10);
        B.enabled = false;

        PlayerPrefs.SetFloat("CampaignCompleted", 10f);

        yield return new WaitForSeconds(3f);
        Debug.Log("Show Now");
        CanReplay = true;
    }

    IEnumerator REPLAYLEVEL()
    {
        Quote.material.DOFade(0f, 5f);
        EyeLid.gameObject.SetActive(true);
        EyeLid.DOLocalMoveY(0.81f, 7f);

        Audio.DOFade(0f, 8f);

        yield return new WaitForSeconds(10f);

        /*
#if UNITY_ANDROID
        AndroidNativePopups.OpenToast("Sprint Unlocked.", AndroidNativePopups.ToastDuration.Short);
#endif
*/

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeJ(bool A, Animator a)
    {
        StartCoroutine(ChangeJump(A, a));
    }

    public static IEnumerator ChangeJump(bool A, Animator a)
    {
        yield return new WaitForSeconds(0.2f);
        a.SetBool("Jump", A);
        if (DEBUG.DoLOG) Debug.Log("Set Jump : " + A);
        a.SetBool("Crouch", A);
        if (DEBUG.DoLOG) Debug.Log("Set Crouch : " + A);
    }

    public IEnumerator SetDieUp()
    {
        yield return new WaitForSeconds(0.2f);
        Anim.SetBool("dieup", false);
    }

    IEnumerator ABC()
    {
        yield return new WaitForSeconds(1);
        Application.Quit();
    }

    public void ShowScore()
    {
        if (HIGHSCORE && ScoreText)
        {
            ScoreText.GetComponent<Text>().text = Mathf.Ceil(PROGRESS.a).ToString();

            //Arcade Mode
            if (GameObject.FindGameObjectWithTag("Arcade") && !Menu.GetComponent<Menu>().isAIMode)
            {
                ScoreText.GetComponent<Text>().text = Mathf.Ceil(PROGRESS.a).ToString() + "m";
                if (PROGRESS.a >= PlayerPrefs.GetFloat("ArcadeHS"))
                {
                    HIGHSCORE.SetActive(true);
                    HIGHSCORE.GetComponent<Text>().text = "New  Highscore\nTap  score  to  share";
                    PlayerPrefs.SetFloat("ArcadeHS", Mathf.Ceil(PROGRESS.a));

                    if (FB.IsLoggedIn)
                    {
                        FBSCRIPT.SetFBHighscore((int)PROGRESS.a);
                        Debug.LogWarning("Updated FB Highscore");
                    }
                }

                else
                {
                    //ScoreText.GetComponent<Button>().enabled = false;
                    HIGHSCORE.GetComponent<Text>().text = "Highscore : " + PlayerPrefs.GetFloat("ArcadeHS") + "m";
                }
            }


            //AI
            else if (GameObject.FindGameObjectWithTag("Arcade") && Menu.GetComponent<Menu>().isAIMode)
            {
                ScoreText.GetComponent<Text>().text = Mathf.Ceil(PROGRESS.a).ToString() + "m";
                if (PROGRESS.a >= PlayerPrefs.GetFloat("AIHS"))
                {
                    HIGHSCORE.SetActive(true);
                    HIGHSCORE.GetComponent<Text>().text = "New  Highscore\nTap  score  to  share";
                    PlayerPrefs.SetFloat("AIHS", Mathf.Ceil(PROGRESS.a));
                    
                    /*
                    if (FB.IsLoggedIn)
                    {
                        FBSCRIPT.SetFBHighscore((int)PROGRESS.a);
                        Debug.LogWarning("Updated FB Highscore");
                    }
                    */
                }

                else
                {
                    //ScoreText.GetComponent<Button>().enabled = false;
                    HIGHSCORE.GetComponent<Text>().text = "Highscore  versus  AI : " + PlayerPrefs.GetFloat("AIHS") + "m";
                }
            }


            //Campaign Mode
            else
            {
                ScoreText.GetComponent<Text>().text = Mathf.Ceil(PROGRESS.a).ToString();
                if (DEBUG.DoLOG) Debug.Log(Mathf.Ceil(PROGRESS.a).ToString());
                HIGHSCORE.SetActive(true);
                HIGHSCORE.GetComponent<Text>().text = "  percent";
            }
        }

        /*
        else
        {
            ScoreText.GetComponent<Text>().text = Mathf.Ceil(PROGRESS.a).ToString() + "m";
            if (PROGRESS.a >= PlayerPrefs.GetFloat("CampaignHS"))
            {
                HIGHSCORE.SetActive(true);
                HIGHSCORE.GetComponent<Text>().text = "";
                PlayerPrefs.SetFloat("CampaignHS", Mathf.Ceil(PROGRESS.a));
            }

            else
            {
                HIGHSCORE.GetComponent<Text>().text = "Highscore : " + PlayerPrefs.GetFloat("CampaignHS");
            }
        }
        */
    }
}
