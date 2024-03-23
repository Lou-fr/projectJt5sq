using System;
using System.Collections.Generic;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

public class FriendsUI : MonoBehaviour
{
    [SerializeField] private Button Friends_Btn, IncomingPending_Btn, Blocked_Btn;
    [SerializeField] private GameObject FriendView,IncomingPendingView,BlockedView;
    private Image Friend_Image, IncomingPending_Image,Blocked_Image;
    private static Color32 SelectColor = new Color32(r:99,g:84,b:76,a:255);
    private static Color32 NotSelected = new Color32(r:108,g:94,b:86,a:255);
    public static Action RefreshFriend =delegate{};
    public static Action PendingIncomingFriend =delegate{};
    public static Action BlockedFriend =delegate{};
    void FriendDisplay()
    {
        if(BlockedView.activeSelf is true) {BlockedView.SetActive(false);Blocked_Image.color = NotSelected;}
        if(IncomingPendingView.activeSelf is true) {IncomingPendingView.SetActive(false);IncomingPending_Image.color = NotSelected;}
        if(FriendView.activeSelf is false) {FriendView.SetActive(true);Friend_Image.color = SelectColor;RefreshFriend?.Invoke();}
        
        
    }
    void IncominPendingDisplay()
    {
        if(BlockedView.activeSelf is true) {BlockedView.SetActive(false);Blocked_Image.color = NotSelected;}
        if(IncomingPendingView.activeSelf is false) {IncomingPendingView.SetActive(true);IncomingPending_Image.color = SelectColor;PendingIncomingFriend?.Invoke();}
        if(FriendView.activeSelf is true) {FriendView.SetActive(false);Friend_Image.color = NotSelected;}
    }
    void BlockedDisplay()
    {
        if(BlockedView.activeSelf is false) {BlockedView.SetActive(true);Blocked_Image.color = SelectColor;BlockedFriend?.Invoke();}
        if(IncomingPendingView.activeSelf is true) {IncomingPendingView.SetActive(false);IncomingPending_Image.color = NotSelected;}
        if(FriendView.activeSelf is true) {FriendView.SetActive(false);Friend_Image.color = NotSelected;}
    }

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

    [SerializeField] private Transform FriendsContainer, blockedContainer,IncomingPendingContainer;
    [SerializeField] FriendPrefab friendPrefab;
    [SerializeField] RequestPrefab requestPrefab;
    [SerializeField] PendingPrefab pendingPrefab;
    [SerializeField] BlockledPrefab blockledPrefab;
    void Awake()
    {
        Friend_Image = Friends_Btn.GetComponentInParent<Image>();
        IncomingPending_Image = IncomingPending_Btn.GetComponentInParent<Image>();
        Blocked_Image = Blocked_Btn.GetComponentInParent<Image>();
        Friends_Btn.onClick.AddListener(FriendDisplay);
        IncomingPending_Btn.onClick.AddListener(IncominPendingDisplay);
        Blocked_Btn.onClick.AddListener(BlockedDisplay);
        FriendsManager.refreshFriends += HandleRefreshFriends;
        FriendsManager.RefreshPendingRequest += HandleRefreshRequest;
        FriendsManager.refreshBlocked += HandleRefreshBlocked;
        RefreshFriend?.Invoke();
    }
    void OnDestroy()
    {
        FriendsManager.refreshFriends -= HandleRefreshFriends;
        FriendsManager.RefreshPendingRequest -= HandleRefreshRequest;
        FriendsManager.refreshBlocked -= HandleRefreshBlocked;

    }

    private void HandleRefreshBlocked(IReadOnlyList<Relationship> list)
    {
        foreach(Transform child in blockedContainer)
        {
            Destroy(child.gameObject);
        }
        foreach(var blocked in list)
        {
            var info = new PlayerProfile(blocked.Member.Profile.Name,blocked.Member.Id);
            BlockledPrefab blockled = Instantiate(blockledPrefab,blockedContainer);
            blockled.Initialize(info);
        }
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

    private void HandleRefreshRequest(List<Member> list,List<Member> list2)
    {
        foreach(Transform child in IncomingPendingContainer)
        {
            Destroy(child.gameObject);
        }
        foreach(var requested in list)
        {
            var info = new PlayerProfile(requested.Profile.Name,requested.Id);
            RequestPrefab request1 = Instantiate(requestPrefab,IncomingPendingContainer);
            request1.Initialize(info);
            FriendsManager.request.Add(info);
        }
        foreach(var requested in list2)
        {
            var info = new PlayerProfile(requested.Profile.Name,requested.Id);
            PendingPrefab pending = Instantiate(pendingPrefab,IncomingPendingContainer);
            pending.Initialize(info);
            FriendsManager.pending.Add(info);
        }
    }

    private void HandleRefreshFriends(List<Member> list)
    {
        foreach (Transform child in FriendsContainer)
        {
            Destroy(child.gameObject);
        }
        foreach(var friend in list)
        {
            string activityText;
            int GamePrivacy = 0;
            if (friend.Presence.Availability == Availability.Offline ||friend.Presence.Availability == Availability.Invisible)
            {
                activityText = ""+friend.Presence.LastSeen.ToLocalTime();
            }else
            {
                var status = friend.Presence.GetActivity<Activity>().Status;
                string[] propieties = status.Split("_");
                activityText = propieties[1];
                string[] secondpropieties = propieties[0].Split(";");
                GamePrivacy = Int32.Parse(secondpropieties[0]);
            }
            var info = new FriendsEntryData
            {
                Name = friend.Profile.Name,
                Id = friend.Id,
                Availability = friend.Presence.Availability,
                Activity = activityText
            };
            FriendsManager.friends.Add(info);
            FriendPrefab friend1 = Instantiate(friendPrefab,FriendsContainer);
            friend1.Initialize(info,GamePrivacy);
        }
    }
}
