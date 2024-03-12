using PlayFab.Networking;
using BleizEntertainment.Multiplayer;
using UnityEngine;
using kcp2k;
public class ServerConnection : MonoBehaviour
{
#if !UNITY_SERVER
    void Start()
    {
        BleizNetworkManager.Instance.networkAddress = Session.Ip;
        BleizNetworkManager.Instance.GetComponent<KcpTransport>().port = (ushort)Session.port;
        BleizNetworkManager.Instance.StartClient();
    }
#endif
}
