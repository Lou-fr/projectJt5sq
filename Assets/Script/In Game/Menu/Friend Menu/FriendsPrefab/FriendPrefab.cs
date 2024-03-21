using System;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

public class FriendPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FriendName, activity;
    [SerializeField] FriendsEntryData Friend;
    [SerializeField] Button Join;
    public static Action<string> RemoveFriend = delegate{};
    public static Action<string> BlockFriend = delegate{};
    public void Initialize(FriendsEntryData friend)
    {
        this.Friend = friend;
        FriendName.SetText(Friend.Name);
        activity.SetText(Friend.Activity);
        if(Friend.Availability == Availability.Offline ||Friend.Availability == Availability.Invisible ||Friend.Availability == Availability.Unknown){Join.interactable = false;}
        else Join.onClick.AddListener(joinReq);
    }
    public void removeFriend()
    {
        RemoveFriend?.Invoke(Friend.Id);
    }
    public void blockFriend()
    {
        BlockFriend?.Invoke(Friend.Id);
    }
    void joinReq()
    {
        FriendsManager.SendjoinRequest(Friend.Id);
    }

}
