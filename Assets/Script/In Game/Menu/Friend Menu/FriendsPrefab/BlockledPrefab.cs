using System;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;

public class BlockledPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] PlayerProfile Blocked;
    public static Action<string> UnBlock = delegate {};
    public void Initialize(PlayerProfile blocked)
    {
        this.Blocked = blocked;
        username.SetText(Blocked.Name);
    }
    public void unblock()
    {
        UnBlock?.Invoke(Blocked.Id);
    }
}
