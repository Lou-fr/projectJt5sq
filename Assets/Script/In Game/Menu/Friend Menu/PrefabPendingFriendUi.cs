using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;

public class PrefabPendingFriendUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FriendName;
    [SerializeField] private FriendInfo friend;

    public static Action<string> CancelFriendReq = delegate { };

    public void Initialize(FriendInfo friend)
    {
        this.friend = friend;
        FriendName.SetText(this.friend.TitleDisplayName);
    }
    public void CancelFriendRequest()
    {
        CancelFriendReq?.Invoke(friend.FriendPlayFabId);
    }
}
