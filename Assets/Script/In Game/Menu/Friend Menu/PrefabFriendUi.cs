using PlayFab.ClientModels;
using System;
using TMPro;
using UnityEngine;

public class PrefabFriendUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI FriendName;
    [SerializeField] private FriendInfo friend;

    public static Action<string> OnRemoveFriend = delegate { };

    public void Initialize(FriendInfo friend)
    {
        this.friend = friend;
        FriendName.SetText(this.friend.TitleDisplayName);
    }
    public void RemoveFriend()
    {
        OnRemoveFriend?.Invoke(friend.FriendPlayFabId);
    }
}
