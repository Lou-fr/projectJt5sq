using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    string lobbyName;
    int maxPlayer = 4;
    static Lobby Curentlobby;
    static PlayerInfo playerInfo;
    bool RelayIsStarted = true;
    bool Initialized = false;
    public static Action<int> LobbyPrivacyStatus = delegate{};

    public static Action SendHostPosition = delegate{};
    public static Action<QueryResponse, Lobby> LobbyRefreshResult = delegate {};
    public static Action<List<Player>> LobbyPlayerResult = delegate{};
    public static Action CreateRelayServer = delegate{};
    public static Action<string> JoinRelayServer = delegate{};
    public static Action StopRelayServer = delegate{};
    public static Action ReadyForDeletetion = delegate{};
    public static Action KickFromLobby = delegate {};
    LobbyEventCallbacks callbacks;
    bool RTMMquitLobby;
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
    
    void Awake()
    {
        Unity_Auth.OnSucess += Initialize;
    }

    private async void Initialize()
    {
        Debug.Log("Initializing lobby manager...",this);
        if(Initialized){Debug.LogWarning("lobby manager already initialized",this);return;}
        if(GameObject.FindObjectsOfType<LobbyManager>().Length > 1){Debug.LogError("another lobby manager already initialized",this);return;}
        playerInfo= await Unity_Auth._GetPlayerInfo();
        LobbyUI.OnRefreshlobby += handleRefreshLobby;
        LobbyUIPrefab.joinLobby += handleJoinLobby;

        FriendsManager.OnJoinRequestAccepted += joinVialobbyCode;
        PlayerManageUIPrefab.KickPlayer += handleKickTargetPlayer;
        LobbyUI.OnRefreshLobbyPlayer += handleRefreshLobbyPlayer;
        LobbyUI.OnRefreshLobbyPrivacy += handleChangePrivacy;
        FirstLoading.StartTransport += handlePrivacyPlayingMode;
        BleizEntertainment.Menu.OnRTMM += handleRTMM;
        HandleCreateLobby();
        Debug.Log("Initialized lobby manager...",this);
    }

    private void handlePrivacyPlayingMode()
    {
        handleChangePrivacy(2);
    }

    private /*async*/ void HandlePlayerOption()
    {
        //When multiplayer across player will be on
        /*UpdatePlayerOptions options = new UpdatePlayerOptions();
        options.Data = new Dictionary<string, PlayerDataObject>()
        {
            {
                "Username",new PlayerDataObject(visibility:PlayerDataObject.VisibilityOptions.Member,value:playerInfo.Username)
            }
        };
        Debug.Log("Uploading Player data");
        await LobbyService.Instance.UpdatePlayerAsync(Curentlobby.Id,playerInfo.Id,options);
        Debug.Log("Upload player data is succes");*/
    }

    private async void HandleCreateLobby()
    {
        if(Curentlobby is not null){if(Curentlobby.HostId == playerInfo.Id)return;}
        callbacks = new LobbyEventCallbacks();
        callbacks.PlayerJoined += handlePlayerJoined;
        lobbyName = playerInfo.Username + "_lobby";
        Curentlobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,maxPlayer,lobbyOptions);
        HandlePlayerOption();
        Relaystart();
        await Lobbies.Instance.SubscribeToLobbyEventsAsync(Curentlobby.Id, callbacks);
        Debug.Log("Lobby started "+Curentlobby.Id);
        StartCoroutine(HeratbeatLobby(Curentlobby.Id,15));
        if(!Initialized){handleChangePrivacy(2);Initialized =true;}
    }

    private void handlePlayerJoined(List<LobbyPlayerJoined> list)
    {
        Debug.Log("Player as joined your game, uploading host postion...");
        //SendHostPosition?.Invoke();
    }
    void OnDestroy()
    {
        Unity_Auth.OnSucess -= Initialize;
        BleizEntertainment.Menu.OnRTMM -= handleRTMM;
        LobbyUI.OnRefreshlobby -= handleRefreshLobby;
        LobbyUIPrefab.joinLobby -= handleJoinLobby;
        FriendsManager.OnJoinRequestAccepted -= joinVialobbyCode;
        LobbyUI.OnRefreshLobbyPlayer -= handleRefreshLobbyPlayer;
        PlayerManageUIPrefab.KickPlayer -= handleKickTargetPlayer;
        LobbyUI.OnRefreshLobbyPrivacy -= handleChangePrivacy;
    }

    private /*async*/ void handleRTMM()
    {
        //if(Curentlobby.HostId != playerInfo.Id){await LobbyService.Instance.RemovePlayerAsync(Curentlobby.Id,playerInfo.Id);RTMMquitLobby=true;return;} ; ENABLE FOR MULTIPLAYER
        handleChangePrivacy(2);  
    }

    private async void handleChangePrivacy(int obj)
    {
        if(playerInfo.Id != Curentlobby.HostId)return;
        UpdateLobbyOptions options = new UpdateLobbyOptions();
        LobbyPrivacyStatus?.Invoke(obj);
        if(obj is 0)
        {
            Debug.Log("Try to put the lobby in public");
            if(Curentlobby.IsPrivate is false && Curentlobby.IsLocked is false)return;
            if(Curentlobby.IsLocked is true)options.IsLocked = false;
            options.IsPrivate = false;
        }else if(obj is 1)
        {
            Debug.Log("Try to put the lobby in friend only");
            if(Curentlobby.IsPrivate is true && Curentlobby.IsLocked is false)return;
            if(Curentlobby.IsLocked is true)options.IsLocked = false;
            if(Curentlobby.IsPrivate is false)options.IsPrivate = true;
        }else
        {
            Debug.Log("Try to put the lobby in private");
            if(Curentlobby.IsPrivate is true && Curentlobby.IsLocked is true)return;
            if(Curentlobby.IsPrivate is false)options.IsPrivate= true;
            options.IsLocked = true;
        }
        Debug.Log(options);
        await LobbyService.Instance.UpdateLobbyAsync(Curentlobby.Id,options);
    }

    private async void handleKickTargetPlayer(string TargetId)
    {
        if(TargetId == Curentlobby.HostId)return;
        await LobbyService.Instance.RemovePlayerAsync(Curentlobby.Id,TargetId);
        Debug.Log(TargetId+" has been kick from "+Curentlobby.Id);
        handleRefreshLobbyPlayer();
    }

    private async void handleRefreshLobbyPlayer()
    {
        if(Curentlobby.HostId != playerInfo.Id)return;
        Curentlobby = await LobbyService.Instance.GetLobbyAsync(Curentlobby.Id);
        LobbyPlayerResult?.Invoke(Curentlobby.Players);
    }

    private async void handleDeleteLobby()
    {
        await LobbyService.Instance.DeleteLobbyAsync(Curentlobby.Id);
        StopAllCoroutines();
        Debug.Log("Delete the lobby with success (lobby id "+Curentlobby.Id+")");
        ReadyForDeletetion?.Invoke();
    }

    private async void HandleJoinCode(string obj)
    {
        try
        {
            Debug.Log("trying to update data lobby");
            UpdateLobbyOptions option = new UpdateLobbyOptions();
            option.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCodeRelay",new DataObject(visibility: DataObject.VisibilityOptions.Member, value:obj)
                }
            };
            await LobbyService.Instance.UpdateLobbyAsync(Curentlobby.Id,option);
            Debug.Log("Data publied");
        }catch (LobbyServiceException e){Debug.LogError(e,this);}
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
                    if(RelayIsStarted is true){StopRelayServer?.Invoke(); RelayIsStarted = false;}
                    StopAllCoroutines();
                    Curentlobby = lobby;
                    HandlePlayerOption();
                    Debug.Log("Joined "+Curentlobby.Id);
                    callbacks = new LobbyEventCallbacks();
                    callbacks.KickedFromLobby += HandleKickFromLobby;
                    callbacks.DataChanged += handleJoinRelay;
                    await Lobbies.Instance.SubscribeToLobbyEventsAsync(lobby.Id, callbacks);
                    handleRefreshLobby();
                    handleJoinRelay(null);
                }catch(LobbyServiceException e){Debug.LogError(e,this);}
            }
        }catch(LobbyServiceException e){Debug.LogError(e,this);}
    }

    private void HandleKickFromLobby()
    {
        if(RTMMquitLobby is true){RTMMquitLobby = false;return;}
        Debug.Log("Has been kick/host as quit the lobby "+Curentlobby.Id);
        KickFromLobby?.Invoke();
        HandleCreateLobby();
    }

    private void handleJoinRelay(Dictionary<string, ChangedOrRemovedLobbyValue<DataObject>> dictionary)
    {
        Debug.Log("Getting relay session code" );
        Curentlobby.Data.TryGetValue("JoinCodeRelay", out DataObject z);
        if(z.Value is null )return;
        Debug.Log("Session code is " + z.Value.ToString());
        JoinRelayServer?.Invoke(z.Value.ToString());
    }

    private void Relaystart()
    {
        if(Curentlobby.HostId != playerInfo.Id) {Debug.Log("You are not the host of " + Curentlobby.Name +"the host is"+Curentlobby.HostId);return;}
        handleRefreshLobby();
        if(RelayIsStarted is false)
        {
            Debug.Log("Player have joined the lobby starting a the relay session");
            CreateRelayServer?.Invoke();
            RelayIsStarted = true;
        }
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
            LobbyRefreshResult?.Invoke(lobbies,Curentlobby);
        }catch (LobbyServiceException e){Debug.LogError(e,this);}
    }

    IEnumerator HeratbeatLobby(string LobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while(true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(LobbyId);
            Debug.Log("Send Heart beat for lobby "+Curentlobby.Id +" lobby name is " + Curentlobby.Name);
            yield return delay;
        }
    }

    async void OnApplicationQuit()
    {
        if(Curentlobby.HostId != playerInfo.Id)
        {
            await LobbyService.Instance.RemovePlayerAsync(Curentlobby.Id, playerInfo.Id);
            Debug.Log("Player leave the lobby");
            return;
        }
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
    public static string GetLobbyCode()
    {
        if(Curentlobby.HostId != playerInfo.Id)return null;
        Debug.Log(Curentlobby.LobbyCode);
        return Curentlobby.LobbyCode;
    }
    private async void joinVialobbyCode(string lobbyCode)
    {
        var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
        await LobbyService.Instance.DeleteLobbyAsync(Curentlobby.Id);
        if(RelayIsStarted is true){StopRelayServer?.Invoke(); RelayIsStarted = false;}
        StopAllCoroutines();
        Curentlobby = lobby;
        HandlePlayerOption();
        callbacks = new LobbyEventCallbacks();
        callbacks.KickedFromLobby += HandleKickFromLobby;
        callbacks.DataChanged += handleJoinRelay;
        await Lobbies.Instance.SubscribeToLobbyEventsAsync(lobby.Id, callbacks);
        handleJoinRelay(null);
    }
    public static async Task<bool> IsCurrentLobbyIsPlayerOwned()
    {
        Curentlobby = await LobbyService.Instance.GetLobbyAsync(Curentlobby.Id);
        if(Curentlobby.HostId == playerInfo.Id)return true;
        return false;
    }
    //Don't work due to API rate limitation 
    /*public static async Task SaveHostPositionAsLobbyData(Vector3 HostPos)
    {
        /*Curentlobby = await LobbyService.Instance.GetLobbyAsync(Curentlobby.Id);
        if(Curentlobby.HostId != playerInfo.Id)return;
        try
        {
            Debug.Log("Try to post the phost position to lobby");
            UpdateLobbyOptions  option = new UpdateLobbyOptions ();
            option.Data = new Dictionary<string, DataObject>()
            {
                {
                    "X_Value",new DataObject(visibility: DataObject.VisibilityOptions.Member, value:HostPos.x.ToString())
                },
                {
                    "Y_Value",new DataObject(visibility: DataObject.VisibilityOptions.Member, value:HostPos.y.ToString())
                },
                {
                    "Z_Value",new DataObject(visibility: DataObject.VisibilityOptions.Member, value:HostPos.z.ToString())
                }
            };
            await LobbyService.Instance.UpdateLobbyAsync(Curentlobby.Id,option);
            Debug.Log("Host position is publied");
        }catch (LobbyServiceException e){Debug.LogError(e);}
    }
    public static Vector3 ReadHostPosition()
    {
        Curentlobby.Data.TryGetValue("X_Value", out DataObject x_valueD);
        Curentlobby.Data.TryGetValue("Y_Value", out DataObject y_valueD);
        Curentlobby.Data.TryGetValue("Z_Value", out DataObject z_valueD);
        if(x_valueD is null && y_valueD is null && z_valueD is null)return Vector3.zero;
        float x_value = x_valueD.ConvertTo<float>();
        float y_value = y_valueD.ConvertTo<float>();
        float z_value = z_valueD.ConvertTo<float>();
        Vector3 temp = new Vector3 (x_value,y_value,z_value);
        return temp;        
    }*/
}
