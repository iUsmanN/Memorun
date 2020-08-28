using UnityEngine;
using System.Collections.Generic;
using Facebook.Unity;

// Class responsible for logging App Events in Friend Smash!
// For more details on App Events see: https://developers.facebook.com/docs/app-events
public static class FBAppEvents
{
    // Constants for a custom App Event name and parameter
    private static readonly string EVENT_NAME_GAME_PLAYED = "game_played";
    private static readonly string EVENT_PARAM_SCORE = "score";

    // Log App Event for app launches
    public static void LaunchEvent()
    {
        // Use ActivateApp to log activation events in your game enabling measurement for App Ads and Facebook Analytics for Apps.
        // https://developers.facebook.com/docs/unity/reference/current/FB.ActivateApp
        FB.ActivateApp();
    }

    // Log a custom App Event for completion of a game with a given score.
    // To understand how to best use App Events see: https://developers.facebook.com/docs/app-events/best-practices
    public static void GameComplete(int score)
    {
        // setup parameters
        var param = new Dictionary<string, object>();
        param[EVENT_PARAM_SCORE] = score;
        // log event
        FB.LogAppEvent(EVENT_NAME_GAME_PLAYED, null, param);
    }

    // Send App Event for spending coins
    public static void SpentCoins(int coins, string item)
    {
        // setup parameters
        var param = new Dictionary<string, object>();
        param[AppEventParameterName.ContentID] = item;
        // log event
        FB.LogAppEvent(AppEventName.SpentCredits, (float)coins, param);
    }
}