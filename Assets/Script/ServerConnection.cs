using PlayFab.Networking;
using BleizEntertainment.Multiplayer;
using UnityEngine;
using kcp2k;
public class ServerConnection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BleizNetworkManager.Instance.networkAddress = Session.Ip;
        BleizNetworkManager.Instance.GetComponent<KcpTransport>().port = (ushort)Session.port;
        BleizNetworkManager.Instance.StartClient();
    }

}
