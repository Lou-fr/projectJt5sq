using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class FriendNetwork : MonoBehaviour
{
    public static Action<FriendInfo> OnDisplayFriends = delegate { };
    public static Action<FriendInfo> OnDisplayPendingFriends = delegate { };
    public static Action<FriendInfo> OnDisplayIncomingFriendsReq = delegate { };
    public static Action DeleteAllChild = delegate { };

    private List<FriendInfo> mFriends;
    private List<FriendInfo> mPendingFriends;
    private void Awake()
    {
        mFriends = new List<FriendInfo>();
        FriendUI.OnAddFriend += HandleAddFriend;
        PrefabFriendUi.OnRemoveFriend += HandleRemoveFriend;
        PrefabIncomeFriendReqUi.OnDeny += HandleRemoveFriend;
        PrefabPendingFriendUi.CancelFriendReq += HandleRemoveFriend;
        PrefabIncomeFriendReqUi.OnAccept += HandleAccpetFrienReq;
        DeleteAllChild?.Invoke();
        GetPlayfabFriend();
    }
    private void OnDestroy()
    {
        FriendUI.OnAddFriend -= HandleAddFriend;
        PrefabFriendUi.OnRemoveFriend -= HandleRemoveFriend;
        PrefabIncomeFriendReqUi.OnDeny -= HandleRemoveFriend;
        PrefabPendingFriendUi.CancelFriendReq -= HandleRemoveFriend;
        PrefabIncomeFriendReqUi.OnAccept -= HandleAccpetFrienReq;
    }

    private void HandleAccpetFrienReq(string id)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "AcceptFriendRequest",
            FunctionParameter = new
            {
                FriendPlayFabId = id
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSucces, OnFailure);
    }

    private void HandleRemoveFriend(string id)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new
            {
                FriendPlayFabId = id
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnRemoveFriendSucess, OnFailure);
        
    }

    private void OnRemoveFriendSucess(ExecuteCloudScriptResult result)
    {
        DeleteAllChild?.Invoke();
        GetPlayfabFriend();
    }


    private void HandleAddFriend(string id)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "SendFriendRequest",
            FunctionParameter = new
            {
                FriendPlayFabId = id
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnAddFriendSucces, OnFailure);
    }

    private void OnAddFriendSucces(ExecuteCloudScriptResult result)
    {
        DeleteAllChild?.Invoke();
        GetPlayfabFriend();
    }

    private void GetPlayfabFriend()
    {
        var request = new GetFriendsListRequest { XboxToken = null};
        PlayFabClientAPI.GetFriendsList(request, OnFriendListSuccess, OnFailure);
    }

    private void OnFriendListSuccess(GetFriendsListResult result)
    {
        mFriends = result.Friends;
        var friendlist = result.Friends;
        foreach (FriendInfo Friend in friendlist)
        {
            foreach(string tag in Friend.Tags)
            {
                if (tag == "requested") OnDisplayPendingFriends?.Invoke(Friend);
                if (tag == "confirmed") OnDisplayFriends?.Invoke(Friend);
                if (tag == "requester") OnDisplayIncomingFriendsReq?.Invoke(Friend);
            }
        }
    }

    private void OnFailure(PlayFabError error)
    {
        Debug.LogWarning(error.ToString());
    }
}
