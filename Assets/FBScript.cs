using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TheNextFlow.UnityPlugins;

public class FBScript : MonoBehaviour
{
    public bool oncefbconnect = false;

    public Menu menu;
    public GameObject FBLeaderBoard;
    public GridLayoutGroup ContainerPanel;
    public GameObject FBLeaderboardPrefab;

    public Button MainFBButton;
    public Text PlayerName;
    public Text MyHS, MyRank;

    public RawImage DP;

    public static string Username;
    public static Texture UserTexture;
    public static List<object> Friends;
    public static Dictionary<string, Texture> FriendImages = new Dictionary<string, Texture>();
    public static List<object> InvitableFriends = new List<object>();

    public static List<object> Scores;

    //My


    void Start()
    {

        //Debug.LogError("Local High Score : " + PlayerPrefs.GetFloat("ArcadeHS"));

        if (!FB.IsInitialized)
        {
            FB.Init();
        }
    }

    public void Reconnect()
    {
        oncefbconnect = false;
        Login();
    }

    public void Login()
    {
        if (!oncefbconnect)
        {
            /*
    #if UNITY_ANDROID
            AndroidNativePopups.OpenToast("Accessing your friends using \"public_profile\" & \"user_friends\" permissions.", AndroidNativePopups.ToastDuration.Short);
    #endif
            */
            //MyPermissions
            var LoginPermissions = new List<string>() { "public_profile", "user_friends" };

            //LoginPrompt
            FB.LogInWithReadPermissions(LoginPermissions, LoginPublish);
        }
    }

    public void LoginPublish(ILoginResult result)
    {
        /*
#if UNITY_ANDROID
        AndroidNativePopups.OpenToast("Updating your score using \"publish_actions\" permission.", AndroidNativePopups.ToastDuration.Short);
#endif
        */
        if (!oncefbconnect)
        {
            if (!FB.IsLoggedIn)
            {
                Debug.Log("NADA");
                menu.OpenHomeArr();
            }

            //MyPermissions
            var LoginPermissions = new List<string> { "publish_actions" };

            //LoginPrompt
            FB.LogInWithPublishPermissions(LoginPermissions, LoginCallBack);

            if(FB.IsLoggedIn)
            oncefbconnect = true;
        }

        if (FB.IsLoggedIn)
            OpenFBLeaderB();
    }

    private void LoginCallBack(ILoginResult result)
    {
        OpenFBLeaderB();

        if (FB.IsLoggedIn)
        {
            //logged in
            if(DEBUG.DoLOG) Debug.LogWarning("FB: LOGGED IN");

            //Download User Data
            GetPlayerInfo();
        }
        else
        {
            /*
#if UNITY_ANDROID
            AndroidNativePopups.OpenToast("Login Failed", AndroidNativePopups.ToastDuration.Short);
#endif
            */
            //user cancelled
            if (DEBUG.DoLOG) Debug.Log("NA");

            if (DEBUG.DoLOG) Debug.Log("NADA");
            menu.OpenHomeArr();
        }
    }

    public void GetPlayerInfo()
    {
        /*
#if UNITY_ANDROID
        AndroidNativePopups.OpenToast("Loading your data using \"public_profile\" permission.", AndroidNativePopups.ToastDuration.Short);
#endif
        */
        string queryString = "/me?fields=id,first_name,picture.width(120).height(120)";
        FB.API(queryString, HttpMethod.GET, GetPlayerInfoCallback);
    }



    private void GetPlayerInfoCallback(IGraphResult result)
    {

        //Get DP
        FB.API(GraphUtil.GetPictureQuery("me", 256, 256), HttpMethod.GET, delegate (IGraphResult dpresult)
        {
            UserTexture = dpresult.Texture;
            if (DEBUG.DoLOG) Debug.LogWarning("FB: GOT DP");
            RedrawUI();
        }
        );

        //Get Name
        string name;
        if (result.ResultDictionary.TryGetValue("first_name", out name))
        {
            if (DEBUG.DoLOG) Debug.LogWarning("FB: GOT NAME");
            Username = name;
            RedrawUI();
        }

        //Get My Highscore
        GetMyHS();




        //Update Screen
        RedrawUI();
    }

    public void GetMyHS()
    {
        FB.API("/app/scores?fields=score,user.limit(10)", HttpMethod.GET, GetScoresCallback);
    }

    public void FBShareGame()
    {
        FB.AppRequest(
    "Come play this great game!",
    null, null, null, null, null, null,
    delegate (IAppRequestResult result) {
        if (DEBUG.DoLOG) Debug.Log(result.RawResult);
    }
);
    }

    private void GetScoresCallback(IGraphResult result)
    {
        var ScoresList = new List<object>();            //Top 10 Scores

        //No Idea
        object scoresh;
        if (result.ResultDictionary.TryGetValue("data", out scoresh))
        {
            ScoresList = (List<object>)scoresh;
        }

        //Remove Prev Entries

        GameObject[] FBElements = GameObject.FindGameObjectsWithTag("FBELEMENT");

        foreach (GameObject g in FBElements)
            Destroy(g);


        //Get Fresh data
        int i = 1;
        foreach (object x in ScoresList)
        {
            var entry = (Dictionary<string, object>)x;
            var user = (Dictionary<string, object>)entry["user"];

            //Debug Details of each User (friend)
            if (DEBUG.DoLOG) Debug.LogError("UN : " + user["name"] + ", Score : " + entry["score"]);

            //Making new entry in the leaderboard
            GameObject X = Instantiate(FBLeaderboardPrefab) as GameObject;
            X.transform.parent = ContainerPanel.transform;          //Setting Container as parent
            X.transform.localScale = new Vector3(1f, 1f, 1f);       //Scaling

            //Setting DP, Rank etc
            X.transform.Find("RankBG").transform.Find("Rank").GetComponent<Text>().text = i.ToString();
            X.transform.Find("Name").GetComponent<Text>().text = user["name"].ToString().Substring(0, user["name"].ToString().IndexOf(' '));
            X.transform.Find("Score").GetComponent<Text>().text = entry["score"].ToString();

            //Getting Friend DP
            FB.API(GraphUtil.GetPictureQuery(user["id"].ToString(), 128, 128), HttpMethod.GET, delegate (IGraphResult dpresult)
            {
                X.transform.Find("FriendDP").GetComponent<RawImage>().texture = dpresult.Texture;
                if (DEBUG.DoLOG) Debug.LogWarning("FB: GOT Friend DP");
                //RedrawUI();
            }
        );
            i++;

            //Me
            if (string.Equals(AccessToken.CurrentAccessToken.UserId, (string)user["id"]))
            {
                MyRank.text = (i-1).ToString();
                //get my score
                int myscore = (int)GraphUtil.GetScoreFromEntry(entry);

                //if local score > online score
                if (myscore < (int)PlayerPrefs.GetFloat("ArcadeHS"))
                {
                    //Update online score (NOT UPLOADED/UPDATED YET)
                    myscore = (int)PlayerPrefs.GetFloat("ArcadeHS");
                }

                //if local score < online score
                else if (myscore >= (int)PlayerPrefs.GetFloat("ArcadeHS"))
                {
                    //Update local score
                    PlayerPrefs.SetFloat("ArcadeHS", myscore * 1.0f);       //Converting from int -> float
                    if (DEBUG.DoLOG) Debug.Log("ARCADE HS : " + PlayerPrefs.GetFloat("ArcadeHS"));
                }

                entry["score"] = myscore;

                //Updating ScoreBug;
                //X.transform.FindChild("Score").GetComponent<Text>().text = myscore.ToString();
                //myscore = X.transform.FindChild("Score").GetComponent<Text>().text;

                //Highlight user
                X.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);

                //Updating HS
                SetFBHighscore(myscore);

                //UpdateUI
                MyHS.text = myscore.ToString();
            }
        }
    }

    public void SetFBHighscore(int a)
    {
        var scoreData = new Dictionary<string, string>();
        scoreData["score"] = a.ToString();

        FB.API("/me/scores", HttpMethod.POST, delegate (IGraphResult result)
        {
            if (DEBUG.DoLOG) Debug.Log("Score submit result: " + result.RawResult);
            /*
#if UNITY_ANDROID
            AndroidNativePopups.OpenToast("COMPLETED Updating your score using \"publish_actions\" permission.", AndroidNativePopups.ToastDuration.Long);
#endif
            */
        }, scoreData);
    }

    IEnumerator ReUpdateUI()
    {
        yield return new WaitForSeconds(0.025f);
    }

    public void OpenFBLeaderB()
    {
        FBLeaderBoard.transform.DOScaleY(1f, 0.02f);
    }

    public void RedrawUI()
    {
        PlayerName.text = Username;
        DP.texture = UserTexture;
        //MyHS.text = PlayerPrefs.GetFloat("ArcadeHS").ToString() + "m";
    }

    //^MYYYYYYYYYYYYYYYYYYYYY

    /*
    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.SetFloat("ArcadeHS", 2f);
        Username = "login";
        if (!FB.IsInitialized)
            FB.Init(InitCallback);
    }

    public void Login()
    {
        //MainFBButton.GetComponent<Button>().interactable = false;

        StartCoroutine(FBCR());
    }

    IEnumerator FBCR()
    {
        FBLogin.PromptForLogin(OnLoginComplete);
        yield return new WaitForSeconds(1.5f);
        if(FB.IsLoggedIn)
        FBLogin.PromptForLogin(OnLoginComplete);
    }

    public void RedrawUI()
    {
        //MainFBButton
        PlayerName.text = Username;
        //MainFBButton.GetComponent<RawImage>().texture = UserTexture;
        DP.texture = UserTexture;

        MyHS.text = PlayerPrefs.GetFloat("ArcadeHS").ToString() + "m";
    }

    #region FB Init
    private void InitCallback()
    {
        Debug.Log("InitCallback");

        // App Launch events should be logged on app launch & app resume
        // See more: https://developers.facebook.com/docs/app-events/unity#quickstart
        //FBAppEvents.LaunchEvent();

        if (FB.IsLoggedIn)
        {
            Debug.Log("Already logged in");
            OnLoginComplete();
        }
    }
    #endregion

    #region Login
    private void OnLoginComplete()
    {
        Debug.Log("OnLoginComplete");

        if (!FB.IsLoggedIn)
        {
            Debug.Log("NOT");
            // Reenable the Login Button
            //MainFBButton.interactable = true;
            return;
        }

        else if (FB.IsLoggedIn)

        {
            Debug.Log("YAS");

            // Show loading animations
            //LoadingText.SetActive(true);

            // Begin querying the Graph API for Facebook data
            FBGraph.GetPlayerInfo();
            FBGraph.GetFriends();
            FBGraph.GetInvitableFriends();
            FBGraph.GetScores();

            RedrawUI();

            FBGraph.GetScores();

            RedrawUI();
            //StartCoroutine(FIXUPDATE());
        }
    }
    #endregion

    IEnumerator FIXUPDATE()
    {

        Debug.Log("FIXUPDATE");
        FBLeaderBoard.SetActive(false);
        yield return new WaitForSeconds(0.01f);
        FBLeaderBoard.SetActive(true);
    }

    public void OpenFBLeaderB()
    {
        FBLeaderBoard.transform.DOScaleY(1f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
