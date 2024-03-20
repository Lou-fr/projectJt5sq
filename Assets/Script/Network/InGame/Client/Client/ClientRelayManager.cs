using System;
using FishNet.Transporting.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ClientRelayManager : MonoBehaviour
{
    JoinAllocation playerAllocation;
    NetworkConnection clientConnection;
    NetworkDriver playerDriver;
	FishyUnityTransport transport;
    public static Action ServerChange = delegate {};
    void Awake()
    {
        LobbyManager.JoinRelayServer += OnJoinServer;
		transport = GetComponentInParent<FishyUnityTransport>();
        if (transport is null) Debug.LogError("Cant get the request comoponent" ,this);
        LobbyManager.KickFromLobby += kickFromLobby;
    }
    void OnDestroy()
    {
        LobbyManager.JoinRelayServer -= OnJoinServer;
        LobbyManager.KickFromLobby -= kickFromLobby;
    }

    private void kickFromLobby()
    {
        transport.StopConnection(false);
    }

    async void OnJoinServer(string Joincode)
    {
    	Debug.Log("Starting allocation for host");
        playerAllocation = await RelayService.Instance.JoinAllocationAsync(Joincode);
        ServerChange?.Invoke();
		Debug.Log("Player Allocation ID: " + playerAllocation.AllocationId);
        Debug.Log("Binding allocation");
        var relayServerData = new RelayServerData(playerAllocation,"dtls");
		transport.StopConnection(true);
        transport.SetRelayServerData(relayServerData);
        transport.StartConnection(false);
    }
    void OnApplicationQuit()
    {
       transport.StopConnection(false);
    }

}
