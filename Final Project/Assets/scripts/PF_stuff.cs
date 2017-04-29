using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Text;
using UnityEngine;

public class PF_stuff : MonoBehaviour
{
    public static readonly Dictionary<string, int> userStatistics = new Dictionary<string, int>();
    public static bool loggedin = false;
    public static string custom_id;
    public static List<PlayerLeaderboardEntry> LB = new List<PlayerLeaderboardEntry>();
    public static string UserId
    {
        get;
        private set;
    }

    private static void OnLoggedIn(LoginResult result)
    {
        UserId = result.PlayFabId;
        print("login successful");
        loggedin = true;
        getleaderboard("Hi");
    }

    private static void OnLoginError(PlayFabError error)
    {
        Debug.LogError("Error logging in player with custom ID:");
        Debug.LogError(error);
        print("login error");
    }

    public static string GetRandomString(System.Random rnd, int length)
    {
        string charPool
        = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvw xyz1234567890";
        StringBuilder rs = new StringBuilder();

        while (length != 0)
        {
            rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);
            length--;
        }

        return rs.ToString();
    }

    public static void login(string x)
    {
        System.Random rnd = new System.Random();
        custom_id = GetRandomString(rnd, 16);
        var request = new LoginWithCustomIDRequest
        {
            TitleId = "792",
            CreateAccount = true,
            CustomId = custom_id
        };
        print("CustomID: ");
        print(custom_id);

        PlayFabClientAPI.LoginWithCustomID(request, OnLoggedIn, OnLoginError);
    }

    public static void UpdateUserStatistics(Dictionary<string, int> updates)
    {
        var request = new UpdatePlayerStatisticsRequest();
        request.Statistics = new List<StatisticUpdate>();

        foreach (var eachUpdate in updates) // Copy the stats from the inputs to the request
        {
            int eachStat;
            userStatistics.TryGetValue(eachUpdate.Key, out eachStat);
            request.Statistics.Add(new StatisticUpdate { StatisticName = eachUpdate.Key, Value = eachUpdate.Value }); // Send the value to the server
            userStatistics[eachUpdate.Key] = eachStat + eachUpdate.Value; // Update the local cache so that future updates are using correct values
        }
        print("points: ");
        print(LevelGoal.points);
        print("wins: ");
        print(LevelGoal.wins);
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateUserStatisticsSuccess, OnUpdateUserStatisticsError);
    }

    private static void OnUpdateUserStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        PF_Bridge.RaiseCallbackSuccess("User Statistics Loaded", PlayFabAPIMethods.UpdateUserStatistics, MessageDisplayStyle.none);
        print("Update User Statistics Success!");

    }

    private static void OnUpdateUserStatisticsError(PlayFabError error)
    {
        PF_Bridge.RaiseCallbackError(error.ErrorMessage, PlayFabAPIMethods.UpdateUserStatistics, MessageDisplayStyle.error);
        print("Update User Statistics Error!");
    }


    public static void getleaderboard(string x)
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "points",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            LB = result.Leaderboard;
            PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.GetFriendsLeaderboard, MessageDisplayStyle.none);
        }, PF_Bridge.PlayFabErrorCallback);

    }
}
