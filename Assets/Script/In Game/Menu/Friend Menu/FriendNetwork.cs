using PlayFab;
using Mirror;
using UnityEngine;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;

public class FriendNetwork : MonoBehaviour
{
    public static Action<List<FriendInfo>> OnFriendListUpdated = delegate { };
    public static Action<List<FriendInfo>> OnDisplayFriends = delegate { };

    private List<FriendInfo> mFriends;
    private void Awake()
    {
        mFriends = new List<FriendInfo>();
        FriendUI.OnAddFriend += HandleAddFriend;
        PrefabFriendUi.OnRemoveFriend += HandleRemoveFriend;
        GetPlayfabFriend();
    }
    private void OnDestroy()
    {
        FriendUI.OnAddFriend -= HandleAddFriend;
        PrefabFriendUi.OnRemoveFriend -= HandleRemoveFriend;
    }

    private void HandleRemoveFriend(string name)
    {
        var request = new RemoveFriendRequest { FriendPlayFabId = name };
        PlayFabClientAPI.RemoveFriend(request, OnRemoveFriendSucess, OnFailure);
    }

    private void OnRemoveFriendSucess(RemoveFriendResult result)
    {
        GetPlayfabFriend();
    }


    private void HandleAddFriend(string name)
    {
        var request = new AddFriendRequest { FriendTitleDisplayName = name };
        PlayFabClientAPI.AddFriend(request, OnAddFriendSucces, OnFailure);
    }

    private void OnAddFriendSucces(AddFriendResult result)
    {
        GetPlayfabFriend();
    }

    private void GetPlayfabFriend()
    {
        var request = new GetFriendsListRequest { XboxToken = null };
        PlayFabClientAPI.GetFriendsList(request, OnFriendListSuccess, OnFailure);
    }

    private void OnFriendListSuccess(GetFriendsListResult result)
    {
        mFriends = result.Friends;
        OnFriendListUpdated?.Invoke(result.Friends);
        OnDisplayFriends?.Invoke(result.Friends);
    }

    private void OnFailure(PlayFabError error)
    {
        Debug.LogWarning(error.ToString());
    }
}
