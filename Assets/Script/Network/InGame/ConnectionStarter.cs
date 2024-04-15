/*#if UNITY_EDITOR
using System;
using FishNet.Transporting.UTP;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionKiller : MonoBehaviour
{
    private FishyUnityTransport _transport;
    void Awake()
    {
        LobbyManager.ReadyForDeletetion += Handle;
    }

    private void Handle()
    {
        Destroy(this.gameObject);
    }
}
#endif

/*using UnityEngine;
using FishNet.Transporting.UTP;

public class ConnectionStarter : MonoBehaviour
{
    private FishyUnityTransport _transport;
    void Start()
    {
        _transport = GetComponentInParent<FishyUnityTransport>();
        _transport.StartConnection(true);
        _transport.StartConnection(false);
    }

}*/