using System;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;

public class FriendPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FriendName, activity;
    [SerializeField] FriendsEntryData Friend;
    public static Action<string> RemoveFriend = delegate{};
    public static Action<string> BlockFriend = delegate{};
    public void Initialize(FriendsEntryData friend)
    {
        this.Friend = friend;
        FriendName.SetText(Friend.Name);
        activity.SetText(Friend.Activity);
    }
    public void removeFriend()
    {
        RemoveFriend?.Invoke(Friend.Id);
    }
    public void blockFriend()
    {
        BlockFriend?.Invoke(Friend.Id);
    }
}
