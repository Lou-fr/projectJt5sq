using System;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public static Action OnRefreshlobby = delegate {};
    [SerializeField] private Transform LobbyContainer;
    [SerializeField] private LobbyUIPrefab lobbyUIPrefab;
    [SerializeField] private Button Refresh;
    void Awake()
    {
        LobbyManager.LobbyRefreshResult += handleDisplayLobby;
        Refresh.onClick.AddListener(OnRefresh);
    }
    
    void OnDestroy()
    {
        LobbyManager.LobbyRefreshResult -= handleDisplayLobby;
        Refresh.onClick.RemoveListener(OnRefresh);
    }

    private void OnRefresh()
    {
        OnRefreshlobby?.Invoke();
    }
    private void handleDisplayLobby(QueryResponse response, string OwnLobby)
    {
        foreach (Transform child in LobbyContainer)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Tous les lobby on été retirer",this);
        foreach (Lobby lobby in response.Results)
        {
            if (lobby.Id != OwnLobby)
            {
                LobbyUIPrefab Lobby = Instantiate(lobbyUIPrefab,LobbyContainer);
                Lobby.Initialize(lobby);
            }
        }
        Debug.Log("Tous les lobby ont été initialiser",this);
    }
}
