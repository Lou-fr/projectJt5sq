namespace PlayFab.Networking
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Mirror;
    using UnityEngine.Events;
    using kcp2k;
    using UnityEngine.SceneManagement;

    public class BleizNetworkManager : NetworkManager
    {
#if !UNITY_SERVER

        public static BleizNetworkManager Instance { get; private set; }

        public ConnectedEvent OnConnected = new ConnectedEvent();
        public DisconnectedEvent OnDisconnected = new DisconnectedEvent();

        public class ConnectedEvent : UnityEvent { }
        public class DisconnectedEvent : UnityEvent<int?> { }

        public override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        public override void Start()
        {
            base.Start();
            this.StartClient();
        }


        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log("client connected");
            OnConnected.Invoke();
        }


        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            Debug.Log("client disconnected");
            OnDisconnected.Invoke(null);
        }

        /// <summary>
        /// Called on clients when a network error occurs.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        public override void OnClientError(TransportError error, string reason) { }

        /// <summary>
        /// Called on clients when a servers tells the client it is no longer ready.
        /// <para>This is commonly used when switching scenes.</para>
        /// </summary>
        /// <param name="conn">Connection to the server.</param>
        public override void OnClientNotReady() { }

        /// <summary>
        /// Called from ClientChangeScene immediately before SceneManager.LoadSceneAsync is executed
        /// <para>This allows client to do work / cleanup / prep before the scene changes.</para>
        /// </summary>
        /// <param name="newSceneName">Name of the scene that's about to be loaded</param>
        /// <param name="sceneOperation">Scene operation that's about to happen</param>
        /// <param name="customHandling">true to indicate that scene loading will be handled through overrides</param>
        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) { }

        /// <summary>
        /// Called on clients when a scene has completed loaded, when the scene load was initiated by the server.
        /// <para>Scene changes can cause player objects to be destroyed. The default implementation of OnClientSceneChanged in the NetworkManager is to add a player object for the connection if no player object exists.</para>
        /// </summary>
        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
        }



        /// <summary>
        /// This is invoked when the client is started.
        /// </summary>
        public override void OnStartClient()
        {
            base.OnStartClient();
        }


        /// <summary>
        /// This is called when a client is stopped.
        /// </summary>
        public override void OnStopClient() { }

    }


    public class CustomGameServerMessageTypes
    {
        public const short ReceiveAuthenticate = 900;
        public const short ShutdownMessage = 901;
        public const short MaintenanceMessage = 902;
    }

    public struct ReceiveAuthenticateMessage : NetworkMessage
    {
        public string PlayFabId;
    }

    public struct ShutdownMessage : NetworkMessage { }

    [Serializable]
    public struct MaintenanceMessage : NetworkMessage
    {
        public DateTime ScheduledMaintenanceUTC;
    }
#endif
#if UNITY_SERVER
        public static BleizNetworkManager Instance { get; private set; }

        public PlayerEvent OnPlayerAdded = new PlayerEvent();
        public PlayerEvent OnPlayerRemoved = new PlayerEvent();

        public int MaxConnections = 4;
        public int Port = 7777; // overwritten by the code in AgentListener.cs

        public List<UnityNetworkConnection> Connections
        {
            get { return _connections; }
            private set { _connections = value; }
        }
        private List<UnityNetworkConnection> _connections = new List<UnityNetworkConnection>();

        public class PlayerEvent : UnityEvent<string> { }

        // Use this for initialization
        public override void Awake()
        {
            base.Awake();
            Instance = this;
            NetworkServer.RegisterHandler<ReceiveAuthenticateMessage>(OnReceiveAuthenticate);
        }

        public void StartListen()
        {
            this.GetComponent<KcpTransport>().port = (ushort)Port;
            NetworkServer.Listen(MaxConnections);
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            NetworkServer.Shutdown();
        }

        private void OnReceiveAuthenticate(NetworkConnection nconn, ReceiveAuthenticateMessage message)
        {
            var conn = _connections.Find(c => c.ConnectionId == nconn.connectionId);
            if (conn != null)
            {
                conn.PlayFabId = message.PlayFabId;
                conn.IsAuthenticated = true;
                OnPlayerAdded.Invoke(message.PlayFabId);
            }
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);

            Debug.LogWarning("Client Connected");
            var uconn = _connections.Find(c => c.ConnectionId == conn.connectionId);
            if (uconn == null)
            {
                _connections.Add(new UnityNetworkConnection()
                {
                    Connection = conn,
                    ConnectionId = conn.connectionId,
                    LobbyId = PlayFabMultiplayerAgentAPI.SessionConfig.SessionId
                });
            }
        }

        public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
        {
            base.OnServerError(conn, error, reason);

            Debug.Log(string.Format("Unity Network Connection Status: exception - {0}", reason));
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);

            var uconn = _connections.Find(c => c.ConnectionId == conn.connectionId);
            if (uconn != null)
            {
                if (!string.IsNullOrEmpty(uconn.PlayFabId))
                {
                    OnPlayerRemoved.Invoke(uconn.PlayFabId);
                }
                _connections.Remove(uconn);
            }
        }
    }

    [Serializable]
    public class UnityNetworkConnection
    {
        public bool IsAuthenticated;
        public string PlayFabId;
        public string LobbyId;
        public int ConnectionId;
        public NetworkConnection Connection;
    }

    public class CustomGameServerMessageTypes
    {
        public const short ReceiveAuthenticate = 900;
        public const short ShutdownMessage = 901;
        public const short MaintenanceMessage = 902;
    }

    public struct ReceiveAuthenticateMessage : NetworkMessage
    {
        public string PlayFabId;
    }

    public struct ShutdownMessage : NetworkMessage { }

    [Serializable]
    public struct MaintenanceMessage : NetworkMessage
    {
        public DateTime ScheduledMaintenanceUTC;
    }
#endif
}
