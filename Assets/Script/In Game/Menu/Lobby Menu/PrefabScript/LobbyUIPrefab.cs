using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyUIPrefab : MonoBehaviour
{
    public static Action<string> joinLobby = delegate {};
    [SerializeField] private TextMeshProUGUI LobbyName;
    [SerializeField] private Lobby lobby;
    public void Initialize(Lobby lobby)
    {
        this.lobby = lobby;
        LobbyName.SetText(this.lobby.Name);
    }
    public void OnJoin()
    {
        joinLobby?.Invoke(lobby.Id);
    }
}
