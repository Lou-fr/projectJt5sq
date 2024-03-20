using System;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;

public class RequestPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requestedName;
    [SerializeField] PlayerProfile profile;
    public static Action<string> DenyFriend = delegate {};
    public static Action<string> AcceptFriend = delegate {};
    public void Initialize(PlayerProfile requested)
    {
        this.profile = requested;
        requestedName.SetText(profile.Name);
    }
    public void denyfriend()
    {
        DenyFriend?.Invoke(profile.Id);
    }
    public void acceptfriend()
    {
        AcceptFriend?.Invoke(profile.Name);
    }

}