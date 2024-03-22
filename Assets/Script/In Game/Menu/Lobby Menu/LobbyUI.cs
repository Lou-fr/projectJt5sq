using System;
using System.Collections.Generic;
using GetLocal;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public static Action OnRefreshlobby = delegate {};
    public static Action OnRefreshLobbyPlayer = delegate{};
    [SerializeField] private Transform LobbyContainer,PlayerContainer;
    [SerializeField] private LobbyUIPrefab lobbyUIPrefab;
    [SerializeField] private PlayerManageUIPrefab PlayerManageUI;
    [SerializeField] private Button Refresh,manage;
    [SerializeField] private GameObject ManagePanel;
    float timeRemaining;
    Lobby currentLobby;
    string temp_content;
    string UserId;
    bool timerRunnig = false;
    TextMeshProUGUI temp_txt;
    async void Awake()
    {
        LobbyManager.LobbyRefreshResult += handleDisplayLobby;
        LobbyManager.KickFromLobby += handleEnableManage;
        LobbyManager.LobbyPlayerResult += handleDisplayPlayer;
        OnRefreshlobby?.Invoke();
        Refresh.onClick.AddListener(OnRefresh);
        Menu.LobbyUIOpen += _OnRefresh;
        manage.onClick.AddListener(ManageUI);
        temp_txt = Refresh.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        temp_content = GetLocalal.GetString("In-Game_Menu","Lobbby_Refresh");
        var t = await AuthenticationService.Instance.GetPlayerInfoAsync();
        UserId = t.Id;
    }
    
    void OnDestroy()
    {
        LobbyManager.LobbyRefreshResult -= handleDisplayLobby;
        LobbyManager.KickFromLobby -= handleEnableManage;
        LobbyManager.LobbyPlayerResult -= handleDisplayPlayer;
        Refresh.onClick.RemoveListener(OnRefresh);
        Menu.LobbyUIOpen -= _OnRefresh;
        manage.onClick.RemoveListener(ManageUI);
    }

    private void handleDisplayPlayer(List<Player> list)
    {
        foreach(Transform child in PlayerContainer)
        {
            Destroy(child.gameObject);
            Debug.Log("Destroying object");
        }
        foreach (Player player in list)
        {
            if(player.Id != UserId)
            {
                PlayerManageUIPrefab prefab = Instantiate(PlayerManageUI,PlayerContainer);
                prefab.Initialize(player,false);
            }else
            {
                PlayerManageUIPrefab prefab = Instantiate(PlayerManageUI,PlayerContainer);
                prefab.Initialize(player,true);
            }
        }
    }

    private void handleEnableManage()
    {
        manage.interactable = true;
    }

    private void OnRefresh()
    {
        OnRefreshlobby?.Invoke();
        Refresh.interactable = false;
        timeRemaining = 5;
        timerRunnig =true;
    }
    private void _OnRefresh(bool Open)
    {
        if(!Open)return;
        OnRefreshlobby?.Invoke();
    }
    private void handleDisplayLobby(QueryResponse response, Lobby currentLoby)
    {
        currentLobby = currentLoby;
        foreach (Transform child in LobbyContainer)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Tous les lobby on été retirer",this);
        foreach (Lobby lobby in response.Results)
        {
            if (lobby.Id != currentLobby.Id)
            {
                LobbyUIPrefab Lobby = Instantiate(lobbyUIPrefab,LobbyContainer);
                Lobby.Initialize(lobby);
            }
        }
        Debug.Log("Tous les lobby ont été initialiser",this);
    }
    void ManageUI()
    {
        if(ManagePanel.activeSelf is true && currentLobby.HostId != UserId){ManagePanel.SetActive(false);manage.interactable = false;return;};
        if(currentLobby.HostId != UserId){manage.interactable = false;return;}
        if(ManagePanel.activeSelf is false) {ManagePanel.SetActive(true);OnRefreshLobbyPlayer?.Invoke();}
        else ManagePanel.SetActive(false);
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
