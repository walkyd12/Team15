using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// PlayerData contains all the PlayFab API calls that relate to manipulating and 
/// </summary>
public static class PF_PlayerData
{
    // Player Level Data:
    public static string PlayerId = string.Empty;
    public static bool showAccountOptionsOnLogin = true;
    public static bool isRegisteredForPush = false;
    public static bool isPlayFabRegistered { get { return accountInfo != null && accountInfo.PrivateInfo != null && !string.IsNullOrEmpty(accountInfo.Username) && !string.IsNullOrEmpty(accountInfo.PrivateInfo.Email); } }
    public static UserAccountInfo accountInfo;
    public static readonly Dictionary<string, UserDataRecord> UserData = new Dictionary<string, UserDataRecord>();

    // this is a sorted, collated structure built from playerInventory. By default, this will only grab items that are in the primary catalog
     public static readonly Dictionary<string, int> virtualCurrency = new Dictionary<string, int>();
    public static readonly List<ItemInstance> playerInventory = new List<ItemInstance>();
    public static readonly Dictionary<string, int> userStatistics = new Dictionary<string, int>();

    //aggregation of player characters
    public static readonly List<CharacterResult> playerCharacters = new List<CharacterResult>();
    public static readonly Dictionary<string, List<string>> characterAchievements = new Dictionary<string, List<string>>();
    public static readonly Dictionary<string, Dictionary<string, int>> characterStatistics = new Dictionary<string, Dictionary<string, int>>();

    public static readonly List<FriendInfo> playerFriends = new List<FriendInfo>();

    public enum PlayerClassTypes { Bucephelous = 0, Nightmare = 1, PegaZeus = 2 }

    // The current character being played:

    #region User Data
    public static void GetUserData(List<string> keys, UnityAction<GetUserDataResult> callback = null)
    {
        var request = new GetUserDataRequest
        {
            Keys = keys,
            PlayFabId = PlayerId,
        };

        //DialogCanvasController.RequestLoadingPrompt (PlayFabAPIMethods.GetUserData);
        PlayFabClientAPI.GetUserReadOnlyData(request, result =>
        {
            if (callback != null)
                callback(result);
            PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.GetUserData, MessageDisplayStyle.none);
        }, PF_Bridge.PlayFabErrorCallback);

    }

    public static void UpdateUserData(Dictionary<string, string> updates, string permission = "Public", UnityAction<UpdateUserDataResult> callback = null)
    {
        var request = new UpdateUserDataRequest
        {
            Data = updates,
            Permission = (UserDataPermission)Enum.Parse(typeof(UserDataPermission), permission),
        };

        PlayFabClientAPI.UpdateUserData(request, result =>
        {
            if (callback != null)
                callback(result);
            PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.UpdateUserData, MessageDisplayStyle.none);
        }, PF_Bridge.PlayFabErrorCallback);
    }

    #endregion

    #region User Statistics
    public static void GetUserStatistics()
    {
        GetPlayerStatisticsRequest request = new GetPlayerStatisticsRequest();
        PlayFabClientAPI.GetPlayerStatistics(request, OnGetUserStatisticsSuccess, OnGetUserStatisticsError);
    }

    private static void OnGetUserStatisticsSuccess(GetPlayerStatisticsResult result)
    {
        //TODO update to use new 

        PF_Bridge.RaiseCallbackSuccess("", PlayFabAPIMethods.GetUserStatistics, MessageDisplayStyle.none);
        foreach (var each in result.Statistics)
            userStatistics[each.StatisticName] = each.Value;
    }

    private static void OnGetUserStatisticsError(PlayFabError error)
    {
        PF_Bridge.RaiseCallbackError(error.ErrorMessage, PlayFabAPIMethods.GetUserStatistics, MessageDisplayStyle.error);
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

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnUpdateUserStatisticsSuccess, OnUpdateUserStatisticsError);
    }

    private static void OnUpdateUserStatisticsSuccess(UpdatePlayerStatisticsResult result)
    {
        PF_Bridge.RaiseCallbackSuccess("User Statistics Loaded", PlayFabAPIMethods.UpdateUserStatistics, MessageDisplayStyle.none);
        GetUserStatistics(); // Refresh stats that we just updated
        Console.Write("Update User Statistics Success!");
    }

    private static void OnUpdateUserStatisticsError(PlayFabError error)
    {
        PF_Bridge.RaiseCallbackError(error.ErrorMessage, PlayFabAPIMethods.UpdateUserStatistics, MessageDisplayStyle.error);
        Console.Write("Update User Statistics Error!");
    }
    #endregion

    #region User Account APIs
   
   
    #endregion

    #region Character APIs
   
    
    #endregion

    #region Inventory Utilities
    /// <summary>
    /// Return number of RemainingUses of an stack of itemIds in your inventory
    /// </summary>
    /// <returns>
    /// -1 => Item does not exist in the inventory
    /// 0 => The item has infinite uses
    /// else, the number of remaining uses
    /// </returns>
    public static int GetItemQty(string itemId)
    {
        var output = 0;
        foreach (var eachItem in playerInventory)
        {
            if (eachItem.ItemId != itemId)
                continue;
            if (eachItem.RemainingUses == null)
                return -1; // Unlimited uses
            if (eachItem.RemainingUses.Value > 0) // Non-Positive is probably a PlayFab api error
                output += eachItem.RemainingUses.Value;
        }
        return output;
    }
    #endregion Inventory Utilities

    #region Friend APIs
    public static void GetFriendsList(UnityAction callback = null)
    {
        var request = new GetFriendsListRequest
        {
            IncludeFacebookFriends = true,
            IncludeSteamFriends = false
        };

        //DialogCanvasController.RequestLoadingPrompt(PlayFabAPIMethods.GetFriendList);
        PlayFabClientAPI.GetFriendsList(request, result =>
        {
            playerFriends.Clear();
            foreach (var eachFriend in result.Friends)
                playerFriends.Add(eachFriend);
            if (callback != null)
                callback();
            PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.GetFriendList, MessageDisplayStyle.none);
        }, PF_Bridge.PlayFabErrorCallback);
    }

    public enum AddFriendMethod { DisplayName, Email, Username, PlayFabID }

    public static void AddFriend(string input, AddFriendMethod method, UnityAction<bool> callback = null)
    {
        var request = new AddFriendRequest();
        switch (method)
        {
            case AddFriendMethod.DisplayName:
                request.FriendTitleDisplayName = input; break;
            case AddFriendMethod.Email:
                request.FriendEmail = input; break;
            case AddFriendMethod.Username:
                request.FriendUsername = input; break;
            case AddFriendMethod.PlayFabID:
                request.FriendPlayFabId = input; break;
        }


        PlayFabClientAPI.AddFriend(request, result =>
        {
            if (callback != null)
                callback(result.Created);
            PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.AddFriend, MessageDisplayStyle.none);
        }, PF_Bridge.PlayFabErrorCallback);
    }

    public static void RemoveFriend(string id, UnityAction callback = null)
    {
        var request = new RemoveFriendRequest { FriendPlayFabId = id };

        PlayFabClientAPI.RemoveFriend(request, result =>
        {
            if (callback != null)
                callback();
            PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.RemoveFriend, MessageDisplayStyle.none);
        }, PF_Bridge.PlayFabErrorCallback);
    }

    public static void SetFriendTags(string id, List<string> tags, UnityAction callback = null)
    {
        var request = new SetFriendTagsRequest
        {
            FriendPlayFabId = id,
            Tags = tags
        };

        PlayFabClientAPI.SetFriendTags(request, result =>
        {
            if (callback != null)
                callback();
            PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.SetFriendTags, MessageDisplayStyle.none);
        }, PF_Bridge.PlayFabErrorCallback);
    }
    #endregion

    #region misc
    public static void RedeemItemOffer(CatalogItem offer, string instanceToRemove, UnityAction<string> callback = null, bool onlyRemoveInstance = false)
    {
        if (onlyRemoveInstance)
        {
            // this offer has already been rewarded, need to remove from the player's invenetory.
            var request = new ExecuteCloudScriptRequest();
            request.FunctionName = "RemoveOfferItem";
            request.FunctionParameter = new { PFID = PlayerId, InstanceToRemove = instanceToRemove };

            PlayFabClientAPI.ExecuteCloudScript(request, result =>
            {
                if (!PF_Bridge.VerifyErrorFreeCloudScriptResult(result))
                    return;

                if (callback != null)
                    callback(null);
                PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.ConsumeOffer, MessageDisplayStyle.none);

            }, PF_Bridge.PlayFabErrorCallback);
        }
        else
        {
            // consume the item 
            var removeReq = new ExecuteCloudScriptRequest
            {
                FunctionName = "RemoveOfferItem",
                FunctionParameter = new { PFID = PlayerId, InstanceToRemove = instanceToRemove }
            };
            PlayFabClientAPI.ExecuteCloudScript(removeReq, result =>
            {
                PF_Bridge.VerifyErrorFreeCloudScriptResult(result);
            }, PF_Bridge.PlayFabErrorCallback);

            // make the award
            var awardRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "RedeemItemOffer",
                FunctionParameter = new { PFID = PlayerId, Offer = offer, SingleUse = offer.Tags.IndexOf("SingleUse") > -1 ? true : false }
            };

            PlayFabClientAPI.ExecuteCloudScript(awardRequest, result =>
            {
                if (!PF_Bridge.VerifyErrorFreeCloudScriptResult(result))
                    return;
                if (callback != null)
                    callback(result.FunctionResult.ToString());
                PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.ConsumeOffer, MessageDisplayStyle.none);
            }, PF_Bridge.PlayFabErrorCallback);
        }
    }
   

  

    public static void RegisterForPushNotification(string pushToken = null, UnityAction callback = null)
    {
#if UNITY_EDITOR || UNITY_EDITOR_OSX
        if (callback != null)
        {
            callback();
            return;
        }
#endif

#if UNITY_IPHONE
			string hexToken = string.Empty;
			byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
			if(token != null)
			{
				RegisterForIOSPushNotificationRequest request = new RegisterForIOSPushNotificationRequest();
				request.DeviceToken = BitConverter.ToString(token).Replace("-", "").ToLower();
				
				hexToken = request.DeviceToken;
				Debug.Log (hexToken);
				
				DialogCanvasController.RequestLoadingPrompt(PlayFabAPIMethods.RegisterForPush);
				PlayFabClientAPI.RegisterForIOSPushNotification(request, result => 
				                                                {
					if(callback != null)
					{
						callback();
					}
					PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.RegisterForPush, MessageDisplayStyle.none);
				}, PF_Bridge.PlayFabErrorCallback);
			}
			else
			{
				Debug.Log("Push Token was null!");
			}
#endif

#if UNITY_ANDROID
        if (!string.IsNullOrEmpty(pushToken))
        {
            Debug.Log("GCM Init Success");
            var request = new AndroidDevicePushNotificationRegistrationRequest { DeviceToken = pushToken };

            DialogCanvasController.RequestLoadingPrompt(PlayFabAPIMethods.RegisterForPush);
            PlayFabClientAPI.AndroidDevicePushNotificationRegistration(request, result =>
            {
                if (callback != null)
                    callback();
                PF_Bridge.RaiseCallbackSuccess(string.Empty, PlayFabAPIMethods.RegisterForPush, MessageDisplayStyle.none);
            }, PF_Bridge.PlayFabErrorCallback);

        }
        else
        {
            Debug.Log("Push Token was null or empty: ");
        }
#endif
    }
    #endregion
}
