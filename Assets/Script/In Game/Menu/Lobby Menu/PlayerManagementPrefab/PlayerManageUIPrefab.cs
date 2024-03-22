using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManageUIPrefab : MonoBehaviour
{
    public static Action<string> KickPlayer=delegate{};
    [SerializeField] private TextMeshProUGUI Username;
    [SerializeField] private Button Kick;
    private Player player;
    public void Initialize(Player Player,bool IsHost)
    {
        if(IsHost)Kick.interactable=false;
        if(!IsHost)Kick.onClick.AddListener(kickPlayer);
        this.player = Player;
        player.Data.TryGetValue("Username",out PlayerDataObject @object);
        Username.SetText(@object.Value.ToString());
    }
    void kickPlayer()
    {
        KickPlayer?.Invoke(player.Id);
    }
    void OnDestroy()
    {
        Kick.onClick.RemoveAllListeners();
    }
    
}
