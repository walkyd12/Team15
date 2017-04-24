using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Text;
using UnityEngine;

public class PF_stuff : MonoBehaviour {
    public static readonly Dictionary<string, int> userStatistics = new Dictionary<string, int>();
    public static int loggedin = 0;
    public static string UserId
    {
        get;
        private set;
    }

    private static void OnLoggedIn(LoginResult result)
    {
        UserId = result.PlayFabId;
        print("login successful");
        loggedin = 1;
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
        string customId = GetRandomString(rnd, 16);
        var request = new LoginWithCustomIDRequest
        {
            TitleId = "792",
            CreateAccount = true,
            CustomId = customId
        };
        print("CustomID: ");
        print(customId);

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
}
