using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;
using UnityEngine;

public class HUDscript : MonoBehaviour {
    public static int points=0;
    public delegate void SuccessfulLoginHandler(string details, MessageDisplayStyle style);
    public static event SuccessfulLoginHandler OnLoginSuccess;
    public ball HugoPrefab;
    public ball ChihuahuaPrefab;
    public ball StBernardPrefab;
    static int sum;
    public static short wins;
    ball Hugo;
    ball Chihuahua;
    ball StBernard;
    Vector3 center;
    Vector3 storage;
    int currentCharacter;
    ball currentBall;
    bool outOfThrows;

    public string UserId
    {
        get;
        private set;
    }

    private static System.Random random = new System.Random();
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private void OnLoggedIn(LoginResult result)
    {
        UserId = result.PlayFabId;
    }

    private static void OnLoginError(PlayFabError error)
    {
        Debug.LogError("Error logging in player with custom ID:");
        Debug.LogError(error);
    }

    private static void OnGetUserStatisticsSuccess(GetPlayerStatisticsResult result)
    {
        //TODO update to use new 

        PF_Bridge.RaiseCallbackSuccess("", PlayFabAPIMethods.GetUserStatistics, MessageDisplayStyle.none);
        foreach (var each in result.Statistics)
            PF_PlayerData.userStatistics[each.StatisticName] = each.Value;
    }

    private static void OnGetUserStatisticsError(PlayFabError error)
    {
        PF_Bridge.RaiseCallbackError(error.ErrorMessage, PlayFabAPIMethods.GetUserStatistics, MessageDisplayStyle.error);
    }

    // Use this for initialization
    void Start () {
        currentCharacter = 0;

         center = new Vector3(0, 0, 0);
        storage = new Vector3(300, 300, 0);
        StBernard = Instantiate(StBernardPrefab, center, transform.rotation);
        Hugo = Instantiate(HugoPrefab, center, transform.rotation);
        Chihuahua = Instantiate(ChihuahuaPrefab, center, transform.rotation);
        storeAllCharacters();
        switchCharacter();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public ball getCurrentBall()
    {
        return currentBall;
    }

    public static void RaiseLoginSuccessEvent(string details, MessageDisplayStyle style)
    {
        if (OnLoginSuccess != null)
            OnLoginSuccess(details, style);
    }
    public void login(string x)
    {
        var request = new LoginWithCustomIDRequest
        {
            TitleId = "792",
            CreateAccount = true,
            CustomId = RandomString(16) // Just a temp value for testing.
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginResult, OnLoginError);


    }

    private static void OnLoginResult(PlayFab.ClientModels.LoginResult result) //LoginResult
    {
        GetPlayerStatisticsRequest request2 = new GetPlayerStatisticsRequest();
        PlayFabClientAPI.GetPlayerStatistics(request2, OnGetUserStatisticsSuccess, OnGetUserStatisticsError);
        PF_PlayerData.PlayerId = result.PlayFabId;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.isEditor)
        {


           }

        PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.GenericLogin, MessageDisplayStyle.none);
        if (OnLoginSuccess != null)

            OnLoginSuccess(string.Format("SUCCESS: {0}", result.SessionTicket), MessageDisplayStyle.error);
    }

    public void playerstatistics(string x)
    {
        GetPlayerStatisticsRequest request2 = new GetPlayerStatisticsRequest();
        PlayFabClientAPI.GetPlayerStatistics(request2, OnGetUserStatisticsSuccess, OnGetUserStatisticsError);
    }

    public void updateplayerstats(string x)
    {
        // playerstatistics("h");
        // foreach(var each in userStatistics)
        // {
        //     print(each.Value);
        // }
        Dictionary<string, int> updates = new Dictionary<string, int>();
        updates.Add("points", 0);
        updates.Add("wins", 0);
        PF_PlayerData.UpdateUserStatistics(updates);
    } 


    public void resetLevel(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
    
        public void switchCharacter()
    {

        bool switched = false;
        int counter = 0;
        if (outOfThrows == false)
        {
            while (switched == false)
            {
                currentCharacter++;
                if (currentCharacter == 1)
                {
                    if (StBernard.getHoldAllowed() == true)
                    {
                        storeCharacter(StBernard);
                    }
                    if (Hugo.getStorage() == true)
                    {
                        centerCharacter(Hugo);
                        currentBall = Hugo;
                        switched = true;
                    }

                }
                else if (currentCharacter == 2)
                {
                    if (Hugo.getHoldAllowed() == true)
                    {
                        storeCharacter(Hugo);
                    }
                    if (Chihuahua.getStorage() == true)
                    {
                        centerCharacter(Chihuahua);
                        currentBall = Chihuahua;
                        switched = true;
                    }

                }
                else
                {
                    if (Chihuahua.getHoldAllowed() == true )
                    {
                        storeCharacter(Chihuahua);
                    }
                    if (StBernard.getStorage() == true)
                    {
                        centerCharacter(StBernard);
                        currentBall = StBernard;
                        switched = true;
                    }
                    currentCharacter = 0;
                }
                counter++;

                if (counter > 3)
                {
                    switched = true;
                    outOfThrows = true;
                }

            }
        }
}
    


    void centerCharacter(ball b)
    {
        b.transform.position = center;
        b.setStorage(0);
    }
    void storeCharacter(ball b)
    {
        b.transform.position = storage;
        b.setStorage(1);
    }
    void storeAllCharacters()
    {
        storeCharacter(Hugo);
        storeCharacter(Chihuahua);
        storeCharacter(StBernard);
    }
}
