using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using FishNet.Transporting.UTP;
using Unity.Networking.Transport.Relay;
using System;
public class RelayHostManager : MonoBehaviour
{
    Allocation HostAllocation;
    string region = null;
    FishyUnityTransport transport;
    public static Action<string> OnJoinCode = delegate{};
    void Awake()
    {
        LobbyManager.CreateRelayServer += StartRelayServer;
        LobbyManager.StopRelayServer += StopRelayServer;
        Menu.OnRTMM += StopRelayServerOnRTMM;
        transport = GetComponentInParent<FishyUnityTransport>();
        if (transport is null) {Debug.LogError("Cant get the request comoponent" ,this);Application.Quit();};  
        StartRelayServer();      
    }
     void OnDestroy()
    {
        LobbyManager.CreateRelayServer -= StartRelayServer;
        LobbyManager.StopRelayServer -= StopRelayServer;
        Menu.OnRTMM -= StopRelayServerOnRTMM;
    }

    private void StopRelayServerOnRTMM()
    {
        transport.ConnectionData = default;
        transport.StopConnection(true);
    }

    private void StopRelayServer()
    {
        transport.ConnectionData = default;
    }

    public async void StartRelayServer()
    {
        Debug.Log("Starting allocation for host");
        int maxConnections = 4;
        HostAllocation = await RelayService.Instance.CreateAllocationAsync(maxConnections,region);
        Debug.Log("Created allocation, allocation ID "+ HostAllocation.AllocationId + " allocation region "+ HostAllocation.Region,this);
        if(region is null) region = HostAllocation.Region;
        Debug.Log("Binding allocation");
        var relayServerData = new RelayServerData(HostAllocation,"dtls");
        transport.SetRelayServerData(relayServerData);
        transport.StartConnection(true);
        transport.StartConnection(false);
        JoinCode();
    }
    private async void JoinCode()
    {
        Debug.Log("Host is getting the join code");
        try
        {
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(HostAllocation.AllocationId);
            Debug.Log("Join code is "+joinCode);
            OnJoinCode?.Invoke(joinCode);
        }catch(RelayServiceException e)
        {
            Debug.LogError(e,this);
        }
    }

    void OnApplicationQuit()
    {
        transport.StopConnection(true);
        transport.StopConnection(false);
    }
}