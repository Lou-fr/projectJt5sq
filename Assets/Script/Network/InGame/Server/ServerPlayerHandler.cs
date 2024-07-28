using Unity.Netcode;
using UnityEngine;

namespace BleizEntertainment
{
    class ServerHandler : NetworkBehaviour
    {
        protected CharacterSOController characterSOController;
        protected NetworkManager networkManager;
        protected void Start()
        {

            if(!IsServer){ return; }
            networkManager = GameObject.FindAnyObjectByType<NetworkManager>().GetComponent<NetworkManager>();
            characterSOController = GetComponent<CharacterSOController>();
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(!IsServer ){Destroy(this.gameObject); return; }
        }
    }
}

