using FishNet.Managing;
using FishNet.Transporting.UTP;
using Unity.Collections;
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
    void Awake()
    {
        LobbyManager.JoinRelayServer += OnJoinServer;
		transport = GetComponentInParent<FishyUnityTransport>();
        if (transport is null) Debug.LogError("Cant get the request comoponent" ,this);
    }
    void OnDestroy()
    {
        LobbyManager.JoinRelayServer -= OnJoinServer;
    }
    async void OnJoinServer(string Joincode)
    {
    	Debug.Log("Starting allocation for host");
        playerAllocation = await RelayService.Instance.JoinAllocationAsync(Joincode);
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
