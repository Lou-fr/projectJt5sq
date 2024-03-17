using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class LobbyManager : MonoBehaviour
{
    string lobbyName;
    int maxPlayer = 4;
    Lobby Curentlobby;
    public static Action<QueryResponse, string> LobbyRefreshResult = delegate {};
    LobbyEventCallbacks callbacks;
    CreateLobbyOptions lobbyOptions = new CreateLobbyOptions()
    {
        IsPrivate = false,
        Data = new Dictionary<string, DataObject>()
        {
            {
                "MinimuLevel", new DataObject(DataObject.VisibilityOptions.Public, value: "1", index:DataObject.IndexOptions.N1)
            },
        }
    };
    
    async void Awake()
    {
        PlayerInfo e = await Unity_Auth._GetPlayerInfo();
        lobbyName = e.Username + "_lobby";
        Curentlobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,maxPlayer,lobbyOptions);
        StartCoroutine(HeratbeatLobby(Curentlobby.Id,15));
        Debug.Log("Lobby started"+Curentlobby.Id);
        LobbyUI.OnRefreshlobby += handleRefreshLobby;
        LobbyUIPrefab.joinLobby += handleJoinLobby;
        callbacks = new LobbyEventCallbacks();
        callbacks.KickedFromLobby += HandleCreateLobby;
        
    }

    private async void HandleCreateLobby()
    {
        callbacks = new LobbyEventCallbacks();
        callbacks.KickedFromLobby += HandleCreateLobby;
        PlayerInfo e = await Unity_Auth._GetPlayerInfo();
        lobbyName = e.Username + "_lobby";
        Debug.Log("Has been kick/host as quit the lobby "+Curentlobby.Id);
        Curentlobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,maxPlayer,lobbyOptions);
        StartCoroutine(HeratbeatLobby(Curentlobby.Id,15));
        Debug.Log("Lobby started "+Curentlobby.Id);
    }

    void OnDestroy()
    {
        LobbyUI.OnRefreshlobby -= handleRefreshLobby;
        LobbyUIPrefab.joinLobby -= handleJoinLobby;

    }

    private async void handleJoinLobby(string LobbyId)
    {
        try
        {
            var lobby = await LobbyService.Instance.GetLobbyAsync(LobbyId);
            if(lobby.AvailableSlots > 0)
            {
                try
                {
                    await LobbyService.Instance.JoinLobbyByIdAsync(LobbyId);
                    Debug.Log("Join the lobby with succes");
                    await LobbyService.Instance.DeleteLobbyAsync(Curentlobby.Id);
                    Debug.Log("Delete the lobby with success (lobby id "+Curentlobby.Id+")");
                    StopAllCoroutines();
                    Curentlobby = lobby;
                    Debug.Log("Joined "+Curentlobby.Id);
                    await Lobbies.Instance.SubscribeToLobbyEventsAsync(lobby.Id, callbacks);
                }catch(LobbyServiceException e){Debug.LogError(e,this);}
            }
        }catch(LobbyServiceException e){Debug.LogError(e,this);}
    }

    private async void handleRefreshLobby()
    {
        try 
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count =25;
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(field: QueryFilter.FieldOptions.AvailableSlots,op: QueryFilter.OpOptions.GT, value: "0")
            };
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(asc: false, field: QueryOrder.FieldOptions.Created)
            };
            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            Debug.Log("Lobby trouvé : "+ lobbies.Results.Count,this);
            LobbyRefreshResult?.Invoke(lobbies,Curentlobby.Id);
        }catch (LobbyServiceException e){Debug.LogError(e,this);}
    }

    IEnumerator HeratbeatLobby(string LobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while(true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(LobbyId);
            Debug.Log("Send Heart beat");
            yield return delay;
        }
    }
    async void OnApplicationQuit()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(Curentlobby.Id);
            Debug.Log("Delete the lobby with success");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }   
    }
}
