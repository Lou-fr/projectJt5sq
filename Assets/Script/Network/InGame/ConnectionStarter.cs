#if UNITY_EDITOR
using FishNet.Transporting.UTP;
using Unity.VisualScripting;
using UnityEngine;

public class ConnectionStarter : MonoBehaviour
{
    private FishyUnityTransport _transport;
    private void Start()
    {
        if(TryGetComponent(out FishyUnityTransport _t))
        {
            _transport = _t;
        }else Debug.LogError("Cant get the request comoponent" ,this);

        if (ParrelSync.ClonesManager.IsClone())
        {
            _transport.StartConnection(false);
        }else
        {
            _transport.StartConnection(true);
            _transport.StartConnection(false);
        }
    }
}
#endif