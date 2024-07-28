using System;
using Unity.Netcode;
using UnityEngine;

namespace BleizEntertainment
{
    public class ServerSOController : NetworkBehaviour
    {
        /*[Rpc(SendTo.SpecifiedInParams)]
        public void spawnDesiredCharacter(NetworkObjectReference RefINet1, ulong ClientId, RpcParams rpcParams, NetworkObject Inet2 = null, NetworkObject Inet3 = null, NetworkObject Inet4 = null)
        {
            try
            {
                NetworkObject INet1 = RefINet1;
                if(Inet2 != null) Inet2.SpawnWithOwnership(ClientId);
                if(Inet3 != null) Inet3.SpawnWithOwnership(ClientId);
                if(Inet4 != null) Inet4.SpawnWithOwnership(ClientId);
                return;
            }
            catch (Exception)
            {
                return;
            }
        }*/
    }
}
