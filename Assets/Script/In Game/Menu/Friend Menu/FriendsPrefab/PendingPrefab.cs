using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;

public class PendingPrefab : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] PlayerProfile pending;
    public static Action<string> CancelOutgoingRequest = delegate {};
    public void Initialize(PlayerProfile Pending)
    {
        this.pending = Pending;
        username.SetText(pending.Name);
    }
    public void canceloutgoingRequest()
    {
        CancelOutgoingRequest?.Invoke(pending.Id);
    }

}
