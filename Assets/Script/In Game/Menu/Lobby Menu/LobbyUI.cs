using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public static Action OnRefreshlobby = delegate {};
    [SerializeField] private Transform LobbyContainer;
    [SerializeField] private LobbyUIPrefab lobbyUIPrefab;
    [SerializeField] private Button Refresh,OpenLobbyUI;
    float timeRemaining;
    string temp_content ="Refresh";
    bool timerRunnig = false;
    TextMeshProUGUI temp_txt;
    void Awake()
    {
        LobbyManager.LobbyRefreshResult += handleDisplayLobby;
        OnRefreshlobby?.Invoke();
        Refresh.onClick.AddListener(OnRefresh);
        OpenLobbyUI.onClick.AddListener(OnRefresh);
        temp_txt = Refresh.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    void OnDestroy()
    {
        LobbyManager.LobbyRefreshResult -= handleDisplayLobby;
        Refresh.onClick.RemoveListener(OnRefresh);
    }

    private void OnRefresh()
    {
        OnRefreshlobby?.Invoke();
        Refresh.interactable = false;
        timeRemaining = 5;
        timerRunnig =true;
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

    private void FixedUpdate()
    {
        if (timerRunnig)
        {
            
            if (timeRemaining > 0.1)
            {
                timeRemaining -= Time.deltaTime;
                temp_txt.text = temp_content + "(" + Mathf.FloorToInt(timeRemaining % 60) +"s)";
            }
            else
            {
                Debug.Log("Timer is out");
                temp_txt.text = temp_content;
                Refresh.interactable=true;
                timerRunnig = false;
            }
        }
    }
}
