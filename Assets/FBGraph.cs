﻿/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using Facebook.Unity;

// Class responsible for Facebook Graph API calls in Friend Smash!
//
// The Facebook Graph API allows us to fetch information about the player and their friends for the permissions
// they grant with Facebook Login.
// We can use this data to provide a set of real people to play against, showing names and pictures
// of the player's friends to make the game experience feel even more personal.
//
// For more details on the Graph API see: https://developers.facebook.com/docs/graph-api/overview
// See https://developers.facebook.com/docs/unity/reference/current/FB.API for Unity specific details
public static class FBGraph
{
    #region PlayerInfo
    // Once a player successfully logs in, we can welcome them by showing their name
    // and profile picture on the home screen of the game. This information is returned
    // via the /me/ endpoint for the current player. We'll call this endpoint via the
    // SDK and use the results to personalize the home screen.
    //
    // Make a Graph API GET call to /me/ to retrieve a player's information
    // See: https://developers.facebook.com/docs/graph-api/reference/user/
    public static void GetPlayerInfo()
    {
        string queryString = "/me?fields=id,first_name,picture.width(120).height(120)";
        FB.API(queryString, HttpMethod.GET, GetPlayerInfoCallback);
    }

    private static void GetPlayerInfoCallback(IGraphResult result)
    {
        if (DEBUG.DoLOG) Debug.Log("GetPlayerInfoCallback");
        if (result.Error != null)
        {
            if (DEBUG.DoLOG) Debug.LogError(result.Error);
            return;
        }
        if (DEBUG.DoLOG) Debug.Log(result.RawResult);

        // Save player name
        string name;
        if (result.ResultDictionary.TryGetValue("first_name", out name))
        {
            FBScript.Username = name;
            if (DEBUG.DoLOG) Debug.Log("USERNAME : " + name);
            FBRedrawUIScript.MyCallUIRedraw();
            GetPlayerPicture();
        }

        //Fetch player profile picture from the URL returned
        string playerImgUrl = GraphUtil.DeserializePictureURL(result.ResultDictionary);
        GraphUtil.LoadImgFromURL(playerImgUrl, delegate (Texture pictureTexture)
        {
            // Setup the User's profile picture
            if (pictureTexture != null)
            {
                FBScript.UserTexture = pictureTexture;
            }

            // Redraw the UI
            if (DEBUG.DoLOG) Debug.Log("Setting DP");
            FBRedrawUIScript.MyCallUIRedraw();
        });

    }

    // In the above request it takes two network calls to fetch the player's profile picture.
    // If we ONLY needed the player's profile picture, we can accomplish this in one call with the /me/picture endpoint.
    //
    // Make a Graph API GET call to /me/picture to retrieve a players profile picture in one call
    // See: https://developers.facebook.com/docs/graph-api/reference/user/picture/
    public static void GetPlayerPicture()
    {
        FB.API(GraphUtil.GetPictureQuery("me", 128, 128), HttpMethod.GET, delegate (IGraphResult result)
        {
            if (DEBUG.DoLOG) Debug.Log("PlayerPictureCallback");
            if (result.Error != null)
            {
                if (DEBUG.DoLOG) Debug.LogError(result.Error);
                return;
            }
            if (result.Texture == null)
            {
                if (DEBUG.DoLOG) Debug.Log("PlayerPictureCallback: No Texture returned");
                return;
            }

            // Setup the User's profile picture
            FBScript.UserTexture = result.Texture;

            // Redraw the UI
            if (DEBUG.DoLOG) Debug.Log("DP");
            FBRedrawUIScript.MyCallUIRedraw();
        });
    }
    #endregion

    #region Friends
    // We can fetch information about a player's friends via the Graph API user edge /me/friends
    // This endpoint returns an array of friends who are also playing the same game.
    // See: https://developers.facebook.com/docs/graph-api/reference/user/friends
    //
    // We can use this data to provide a set of real people to play against, showing names
    // and pictures of the player's friends to make the experience feel even more personal.
    //
    // The /me/friends edge requires an additional permission, user_friends. Without
    // this permission, the response from the endpoint will be empty. If we know the user has
    // granted the user_friends permission but we see an empty list of friends returned, then
    // we know that the user has no friends currently playing the game.
    //
    // Note:
    // In this instance we are making two calls, one to fetch the player's friends who are already playing the game
    // and another to fetch invitable friends who are not yet playing the game. It can be more performant to batch 
    // Graph API calls together as Facebook will parallelize independent operations and return one combined result.
    // See more: https://developers.facebook.com/docs/graph-api/making-multiple-requests
    //
    public static void GetFriends()
    {
        string queryString = "/me/friends?fields=id,first_name,picture.width(128).height(128)&limit=100";
        FB.API(queryString, HttpMethod.GET, GetFriendsCallback);
    }

    private static void GetFriendsCallback(IGraphResult result)
    {
        if (DEBUG.DoLOG) Debug.Log("GetFriendsCallback");
        if (result.Error != null)
        {
            if (DEBUG.DoLOG) Debug.LogError(result.Error);
            return;
        }
        if (DEBUG.DoLOG) Debug.Log(result.RawResult);

        // Store /me/friends result
        object dataList;
        if (result.ResultDictionary.TryGetValue("data", out dataList))
        {
            var friendsList = (List<object>)dataList;
            CacheFriends(friendsList);
        }
    }

    // We can fetch information about a player's friends who are not yet playing our game
    // via the Graph API user edge /me/invitable_friends
    // See more about Invitable Friends here: https://developers.facebook.com/docs/games/invitable-friends
    //
    // The /me/invitable_friends edge requires an additional permission, user_friends.
    // Without this permission, the response from the endpoint will be empty.
    //
    // Edge: https://developers.facebook.com/docs/graph-api/reference/user/invitable_friends
    // Nodes returned are of the type: https://developers.facebook.com/docs/graph-api/reference/user-invitable-friend/
    // These nodes have the following fields: profile picture, name, and ID. The ID's returned in the Invitable Friends
    // response are not Facebook IDs, but rather an invite tokens that can be used in a custom Game Request dialog.
    //
    // Note! This is different from the following Graph API:
    // https://developers.facebook.com/docs/graph-api/reference/user/friends
    // Which returns the following nodes:
    // https://developers.facebook.com/docs/graph-api/reference/user/
    //
    public static void GetInvitableFriends()
    {
        string queryString = "/me/invitable_friends?fields=id,first_name,picture.width(128).height(128)&limit=100";
        FB.API(queryString, HttpMethod.GET, GetInvitableFriendsCallback);
    }

    private static void GetInvitableFriendsCallback(IGraphResult result)
    {
        if (DEBUG.DoLOG) Debug.Log("GetInvitableFriendsCallback");
        if (result.Error != null)
        {
            if (DEBUG.DoLOG) Debug.LogError(result.Error);
            return;
        }
        if (DEBUG.DoLOG) Debug.Log(result.RawResult);

        // Store /me/invitable_friends result
        object dataList;
        if (result.ResultDictionary.TryGetValue("data", out dataList))
        {
            var invitableFriendsList = (List<object>)dataList;
            CacheFriends(invitableFriendsList);
        }
    }

    private static void CacheFriends(List<object> newFriends)
    {
        if (FBScript.Friends != null && FBScript.Friends.Count > 0)
        {
            FBScript.Friends.AddRange(newFriends);
        }
        else
        {
            FBScript.Friends = newFriends;
        }
    }
    #endregion

    #region Scores
    // Fetch leaderboard scores from Scores API
    // Scores API documentation: https://developers.facebook.com/docs/games/scores
    //
    // With player scores being written to the Graph API, we now have a data set on
    // which to build a social leaderboard. By calling the /app/scores endpoint for
    // your app, with a user access token, you get back a list of the current player's
    // friends' scores, ordered by score.
    //
    public static void GetScores()
    {
        FB.API("/app/scores?fields=score,user.limit(20)", HttpMethod.GET, GetScoresCallback);
    }

    private static void GetScoresCallback(IGraphResult result)
    {
        if (DEBUG.DoLOG) Debug.Log("GetScoresCallback");
        if (result.Error != null)
        {
            if (DEBUG.DoLOG) Debug.LogError(result.Error);
            return;
        }
        if (DEBUG.DoLOG) Debug.Log(result.RawResult);

        // Parse scores info
        var scoresList = new List<object>();

        object scoresh;
        if (result.ResultDictionary.TryGetValue("data", out scoresh))
        {
            scoresList = (List<object>)scoresh;
        }

        // Parse score data
        HandleScoresData(scoresList);

        // Redraw the UI
        FBRedrawUIScript.CallUIRedraw();
    }

    private static void HandleScoresData(List<object> scoresResponse)
    {
        var structuredScores = new List<object>();
        foreach (object scoreItem in scoresResponse)
        {
            // Score JSON format
            // {
            //   "score": 4,
            //   "user": {
            //      "name": "Chris Lewis",
            //      "id": "10152646005463795"
            //   }
            // }

            var entry = (Dictionary<string, object>)scoreItem;
            var user = (Dictionary<string, object>)entry["user"];
            string userId = (string)user["id"];

            if (string.Equals(userId, AccessToken.CurrentAccessToken.UserId))
            {
                // This entry is the current player
                int playerHighScore = GraphUtil.GetScoreFromEntry(entry);
                if (DEBUG.DoLOG) Debug.Log("Local players score on server is " + playerHighScore);

                //localhs
                int a = (int)PlayerPrefs.GetFloat("ArcadeHS");
                if (DEBUG.DoLOG) Debug.Log("LocalHS : " + a);
                if (playerHighScore <= a)
                {
                    if (DEBUG.DoLOG) Debug.Log("Locally overriding with just acquired score: " + PROGRESS.a);
                    playerHighScore = (int) PlayerPrefs.GetFloat("ArcadeHS");
                }

                else if (playerHighScore >= PlayerPrefs.GetFloat("ArcadeHS"))
                {
                    PlayerPrefs.SetFloat("ArcadeHS", (float)playerHighScore);
                }

                entry["score"] = playerHighScore.ToString();
                //FBScript.HighScore = playerHighScore;
                PlayerPrefs.SetFloat("ArcadeHS", (float)playerHighScore);
                FBRedrawUIScript.MyCallUIRedraw();
            }

            structuredScores.Add(entry);
            if (!FBScript.FriendImages.ContainsKey(userId))
            {
                // We don't have this players image yet, request it now
                LoadFriendImgFromID(userId, pictureTexture =>
                {
                    if (pictureTexture != null)
                    {
                        FBScript.FriendImages.Add(userId, pictureTexture);
                        FBRedrawUIScript.CallUIRedraw();
                    }
                });
            }
        }

        FBScript.Scores = structuredScores;
    }

    // Graph API call to fetch friend picture from user ID returned from FBGraph.GetScores()
    //
    // Note: /me/invitable_friends returns invite tokens instead of user ID's,
    // which will NOT work with this /{user-id}/picture Graph API call.
    private static void LoadFriendImgFromID(string userID, Action<Texture> callback)
    {
        FB.API(GraphUtil.GetPictureQuery(userID, 128, 128),
               HttpMethod.GET,
               delegate (IGraphResult result)
               {
                   if (result.Error != null)
                   {
                       if (DEBUG.DoLOG) Debug.LogError(result.Error + ": for friend " + userID);
                       return;
                   }
                   if (result.Texture == null)
                   {
                       if (DEBUG.DoLOG) Debug.Log("LoadFriendImg: No Texture returned");
                       return;
                   }
                   callback(result.Texture);
               });
    }
    #endregion
}
