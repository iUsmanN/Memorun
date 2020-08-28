using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace Lean.Touch
{
    // This script will tell you which direction you swiped in
    public class LeanSwipeDirection4 : NetworkBehaviour
    {
        [Tooltip("The text element we will display the swipe information in")]
        public Text InfoText;
        public Rigidbody Char;
        public OnGround CharHere;
        public Animator Anim;
        public Collider LowCol;
        public move Move;
        public follow Cam;
        public React ReactS;
        public PlsyerAI AI;
        public Transform ParticleFX;
        public GameObject ParticleSpeed;
        public GameObject ParticSys;

        public AudioSource AudioFX;
        public AudioClip JumpClip;

        public bool speeding, slowing;
        public Transform SpeedUpT, SlowDownT;

        //        bool Speeding = false;

        protected virtual void OnEnable()
        {
            // Hook into the events we need
            LeanTouch.OnFingerSwipe += OnFingerSwipe;
        }

        void Start()
        {
            speeding = slowing = false;
            React.isPlaying = false;
            if (DEBUG.DoLOG) Debug.Log("isPlaying = false");

            if (ParticleSpeed)
                ParticleSpeed.SetActive(false);

            if (PlayerPrefs.GetFloat("CampaignCompleted") != 0f)
            {
                SpeedUpT.gameObject.GetComponent<Text>().text = "Sprint";
                SlowDownT.gameObject.GetComponent<Text>().text = "Slow   motion";
            }
        }

        public System.Collections.IEnumerator SlowMo()
        {
            if (PlayerPrefs.GetFloat("CampaignCompleted") != 0f && (!Anim.GetBool("die") && !Anim.GetBool("dieup")))
            {
                slowing = true;
                Time.timeScale = 0.5f;
                Move.gameObject.GetComponent<ColorSystem>().DefColor(2f);

                Camera.main.transform.DOLocalMoveZ(-0.048f, 1f);
                yield return new WaitForSeconds(1.5f);

                

                if (!Anim.GetBool("die") && !Anim.GetBool("dieup"))
                {
                    Camera.main.transform.DOLocalMoveZ(-0.077f, 1f);
                    yield return new WaitForSeconds(0.8f);

                    Time.timeScale = 1.2f;
                    slowing = false;
                }
            }

            else if (PlayerPrefs.GetFloat("CampaignCompleted") == 0f && (!Anim.GetBool("die") && !Anim.GetBool("dieup")))
            {
                Camera.main.DOFieldOfView(58f, 0.2f);
                yield return new WaitForSeconds(0.2f);
                Camera.main.DOFieldOfView(62f, 0.2f);
                yield return new WaitForSeconds(0.2f);
                Camera.main.DOFieldOfView(60f, 0.2f);
            }
        }

        public System.Collections.IEnumerator SpeedUpTCR()
        {
            if (!Anim.GetBool("die") && !Anim.GetBool("dieup"))
            {
                if (SlowDownT)
                    SlowDownT.DOScaleY(0f, 0.5f);

                if (SpeedUpT)
                    SpeedUpT.DOScaleY(0.01529f, 0.5f);

                yield return new WaitForSeconds(2f);

                if (SpeedUpT)
                    SpeedUpT.DOScaleY(0f, 0.5f);
            }
        }

        public System.Collections.IEnumerator SLowDownTCR()
        {
            if (!Anim.GetBool("die") && !Anim.GetBool("dieup"))
            {
                if (SpeedUpT)
                    SpeedUpT.DOScaleY(0f, 0.5f);

                if (SlowDownT)
                    SlowDownT.DOScaleY(0.01529f, 0.5f);

                yield return new WaitForSeconds(1.7f);

                if (SlowDownT)
                    SlowDownT.DOScaleY(0f, 0.5f);
            }
        }

        public System.Collections.IEnumerator SpeedUp()
        {
            if (PlayerPrefs.GetFloat("CampaignCompleted") == 0f && (!Anim.GetBool("die") && !Anim.GetBool("dieup")))
            {
                Camera.main.DOFieldOfView(58f, 0.2f);
                yield return new WaitForSeconds(0.2f);
                Camera.main.DOFieldOfView(62f, 0.2f);
                yield return new WaitForSeconds(0.2f);
                Camera.main.DOFieldOfView(60f, 0.2f);
            }

            else if (PlayerPrefs.GetFloat("CampaignCompleted") != 0f && (!Anim.GetBool("die") && !Anim.GetBool("dieup")))
            {
                speeding = true;

                if (ParticleSpeed)
                    ParticleSpeed.SetActive(true);

                Move.Sp = 10f;
                Move.gameObject.GetComponent<ColorSystem>().DefColor(2f);
                Anim.speed = 1.2f;

                //ParticleSystem ps = ParticSys.GetComponent<ParticleSystem>();
                //ParticSys.GetComponent<ParticleSystemRenderer>().renderMode = ParticleSystemRenderMode.Stretch;
                //ParticSys.GetComponent<ParticleSystemRenderer>().velocityScale = Mathf.Lerp(0f, 1f, 0.2f); //3f;
                //Mathf.Lerp(ParticSys.GetComponent<ParticleSystemRenderer>().lengthScale, 15f, 0.5f);
                //if(!slowing)
                Camera.main.DOFieldOfView(68f, 1f);

                yield return new WaitForSeconds(1.5f);

                //if(!slowing)
                Camera.main.DOFieldOfView(60f, 1f);

                yield return new WaitForSeconds(0.8f);
                //ParticSys.GetComponent<ParticleSystemRenderer>().velocityScale = Mathf.Lerp(1f, 0f, 0.2f); //3f;
                //Mathf.Lerp(ParticSys.GetComponent<ParticleSystemRenderer>().lengthScale, 1f, 0.5f);

                if (!Anim.GetBool("die") && !Anim.GetBool("dieup"))
                {
                    Move.Sp = 8f;
                    Anim.speed = 1f;
                }

                if (ParticleSpeed)
                    ParticleSpeed.SetActive(false);

                speeding = false;
            }
        }

        protected virtual void OnDisable()
        {
            // Unhook the events
            LeanTouch.OnFingerSwipe -= OnFingerSwipe;
        }

        public void OnFingerSwipe(LeanFinger finger)
        {
            // Make sure the info text exists
            if (InfoText != null)
            {
                // Store the swipe delta in a temp variable
                var swipe = finger.SwipeScreenDelta;

                if (swipe.x < -Mathf.Abs(swipe.y))
                {
                    //InfoText.text = "You swiped left!";
                    //if(AI)
                    //AI.enabled = false;

                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    //Mathf.Lerp(Move.Sp, 8f, 1f);
                    if (React.isPlaying)
                    {
                        if (DEBUG.DoLOG) Debug.Log("StartCoroutine(SlowMo());");
                        StartCoroutine(SlowMo());
                        StartCoroutine(SLowDownTCR());
                    }

                    //Char.gameObject.transform.position = new Vector3(1421.4f, Char.gameObject.transform.position.y, Char.gameObject.transform.position.z);

                }

                if (swipe.x > Mathf.Abs(swipe.y))
                {
                    //InfoText.text = "You swiped right!";
                    if (DEBUG.DoLOG) Debug.Log("RIGHT SWIPED");

                    //if(!Speeding)
                    //Move.SpeedUP();

                    if (React.isPlaying)
                    {
                        StartCoroutine(SpeedUp());
                        StartCoroutine(SpeedUpTCR());
                        if (DEBUG.DoLOG) Debug.Log("StartCoroutine(SpeedUp());");
                    }

                    //if(AI)
                    //AI.enabled = true;
                    //SceneManager.LoadScene(1);
                    //Mathf.Lerp(Move.Sp, 11f, 1f);

                }

                if (swipe.y < -Mathf.Abs(swipe.x))
                {
                    //InfoText.text = "You swiped down!";
                    if (!Anim)
                    {
                        GameObject[] x = GameObject.FindGameObjectsWithTag("MP");

                        foreach (GameObject g in x)
                            if (g.GetComponent<SettingMyNetwork>().AmILocal())
                                Anim = g.GetComponent<Animator>();
                    }

                    Anim.SetFloat("JumpVar", Random.Range(0f, 10f));
                    Anim.SetBool("Crouch", true);
                    ReactS.ChangeJ(false, Anim);
                    //Move.Sp = 5f;
                }

                if (swipe.y > Mathf.Abs(swipe.x))
                {
                    if (!Anim)
                    {
                        GameObject[] x = GameObject.FindGameObjectsWithTag("MP");

                        foreach (GameObject g in x)
                            if (g.GetComponent<SettingMyNetwork>().AmILocal())
                                Anim = g.GetComponent<Animator>();
                    }
                    //PlayerPrefs.SetInt("Tutorial", 1);
                    Anim.SetFloat("JumpVar", Random.Range(0f, 10f));
                    //InfoText.text = "You swiped up!";
                    if (DEBUG.DoLOG) Debug.Log("Swiped UP!");
                    /* if (CharHere.GetGroundVar())
                     {*/
                    //Char.AddForce(new Vector3(0f, 10000f, 0f));
                    //Set Anim Jump Bool Here

                    Anim.SetBool("Jump", true);

                    //LowCol.enabled = false;
                    ReactS.ChangeJ(false, Anim);
                    //Cam.JumpCam();
                    // }
                    if (AudioFX)
                        AudioFX.PlayOneShot(JumpClip, 0.5f);
                }
            }
        }
    }
}