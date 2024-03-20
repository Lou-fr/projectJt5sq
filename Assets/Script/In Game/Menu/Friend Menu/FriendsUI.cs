using System;
using UnityEngine.Scripting;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;

public class FriendsUI : MonoBehaviour
{
    [SerializeField] private string FriendNameOrId;
    public static Action<string> OnAddFriend = delegate {};

    public void SetFriendName(string name)
    {
        FriendNameOrId = name;
    }
    public void AddFriend()
    {
        if(string.IsNullOrEmpty(FriendNameOrId))return;
        OnAddFriend?.Invoke(FriendNameOrId);
    }

    public static Action refresh = delegate{};
    [SerializeField] private Transform FriendsContainer;
    [SerializeField] FriendPrefab friendPrefab;
    [SerializeField] RequestPrefab requestPrefab;
    [SerializeField] PendingPrefab pendingPrefab;
    void Awake()
    {
        FriendsManager.refreshFriends += HandleRefreshFriends;
        FriendsManager.refreshRequest += HandleRefreshRequest;
        FriendsManager.ClearList += HandleClearFriendList;
        FriendsManager.refreshPending += HandleOutgoingRequest;
        refresh?.Invoke();
    }
    void OnDestroy()
    {
        FriendsManager.refreshFriends -= HandleRefreshFriends;
        FriendsManager.refreshRequest -= HandleRefreshRequest;
        FriendsManager.ClearList -= HandleClearFriendList;
        FriendsManager.refreshPending += HandleOutgoingRequest;
    }

    private void HandleOutgoingRequest(List<Member> list)
    {
        foreach(var requested in list)
        {
            var info = new PlayerProfile(requested.Profile.Name,requested.Id);
            PendingPrefab pending = Instantiate(pendingPrefab,FriendsContainer);
            pending.Initialize(info);
            FriendsManager.pending.Add(info);
        }
    }

    private void HandleClearFriendList()
    {
        foreach (Transform child in FriendsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void HandleRefreshRequest(List<Member> list)
    {
        foreach(var requested in list)
        {
            var info = new PlayerProfile(requested.Profile.Name,requested.Id);
            RequestPrefab request1 = Instantiate(requestPrefab,FriendsContainer);
            request1.Initialize(info);
            FriendsManager.request.Add(info);
        }
    }

    private void HandleRefreshFriends(List<Member> list)
    {
        
        foreach(var friend in list)
        {
            string activityText;
            if (friend.Presence.Availability == Availability.Offline ||friend.Presence.Availability == Availability.Invisible)
            {
                activityText = ""+friend.Presence.LastSeen.ToLocalTime();
            }else
            {
                activityText = friend.Presence.Availability.ToString();
            }
            var info = new FriendsEntryData
            {
                Name = friend.Profile.Name,
                Id = friend.Id,
                Availability = friend.Presence.Availability,
                Activity = activityText
            };
            FriendPrefab friend1 = Instantiate(friendPrefab,FriendsContainer);
            friend1.Initialize(info);
            FriendsManager.friends.Add(info);
        }
    }

}
