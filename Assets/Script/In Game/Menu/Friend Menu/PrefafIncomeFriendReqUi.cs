using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;

public class PrefabIncomeFriendReqUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FriendName;
    [SerializeField] private FriendInfo friend;

    public static Action<string> OnDeny = delegate { };
    public static Action<string> OnAccept = delegate { };

    public void Initialize(FriendInfo friend)
    {
        this.friend = friend;
        FriendName.SetText(this.friend.TitleDisplayName);
    }
    public void Deny()
    {
        OnDeny?.Invoke(friend.FriendPlayFabId);
    }
    public void accept()
    {
        OnAccept?.Invoke(friend.FriendPlayFabId);
    }
}
