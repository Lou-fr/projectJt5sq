using System;
using UnityEngine;

namespace BleizEntertainment
{
#if BleizOnline
    public class CharacterSOController : NetworkBehaviour
    {
        protected CharacterDictionary characterDictionary = new();
        private PlayerHandler playerHandler;
        public PlayerSO[] ListCurrentChara { get; protected set; } = new PlayerSO[4];
        bool Iniated = false;
        int CharacterNumber = 0;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (!IsOwner || IsServer) return;
            characterDictionary.Initialize();

            playerHandler = GetComponent<PlayerHandler>();
            /*var temp = GetCurentCharacter();*/
        }
        private void AddRowCharacter(int[] CharcterList)
        {
            for (int i = 0; i < CharcterList.Length; i++)
            {
                AddCharacterToList(CharcterList[i], i);
            }

        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

        }
        public int[] GetListCurrentCharacter()
        {
            int[] t = { 2410, 13, 857 };
            return t;
        }
        private (bool, PlayerSO) ChekDuplicate(PlayerSO playerSO, PlayerSO[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == playerSO)
                {
                    return (true, null);
                }
            }
            return (false, playerSO);
        }
        private void AddCharacterToList(int id, int position)
        {
            try
            {
                PlayerSO temp = characterDictionary.GetPlayerSOFromId(id);
                if (ChekDuplicate(temp, ListCurrentChara) != (true, null))
                {
                    CharacterNumber++;
                    ListCurrentChara[position] = temp;
                    if (IsServer) Debug.Log($"CHARCTER SWAP SYS | SERVER SIDED : Added {temp.CharacterInfoData.ChatacterName} to position {position}", this);
                    if (IsClient) Debug.Log($"CHARCTER SWAP SYS | CLIENT SIDED : Added {temp.CharacterInfoData.ChatacterName} to position {position}", this);
                }
                else
                {
                    if (IsServer) Debug.LogWarning($"CHARCTER SWAP SYS | SERVER SIDED : Duplicate character ({temp.CharacterInfoData.ChatacterName} at position {position}), this character is not added", this);
                    if (IsClient) Debug.LogWarning($"CHARCTER SWAP SYS | CLIENT SIDED : Duplicate character ({temp.CharacterInfoData.ChatacterName} at position {position}), this character is not added", this);
                }
            }
            catch (Exception e)
            {
                if (IsServer) Debug.LogError($"CHARCTER SWAP SYS | SERVER SIDED : {e}", this);
                if (IsClient) Debug.LogError($"CHARCTER SWAP SYS | CLIENT SIDED : {e}", this);
            }
        }
        /*public void SpawnCharacter(GameObject[] Character,GameObject parent, ulong senderId)
        {
            if (!IsOwner) return;
            for (int i = 0; i < Character.Length; i++)
            {
                var tarCharcter = Character[i].GetComponent<NetworkObject>();
                ServerSpawnCharacterRpc(tarCharcter,parent, senderId,i);
            }
        }*/
        #region Remote Procedure Call (RPC)
        /*[Rpc(SendTo.Server)]
        public void ServerSpawnCharacterRpc(NetworkObjectReference RefInet, NetworkObjectReference parents, ulong senderId,int index)
        {
            bool success = false;
            try
            {
                NetworkObject InetObj = RefInet;
                InetObj.SpawnWithOwnership(senderId);
                GameObject parent = parents;
                InetObj.transform.SetParent(parent.transform);
            }
            catch (Exception  e) 
            {
                success = false;
                Debug.Log(e, this);
            }
            Debug.Log(success, this);
            ClientSpawnCharacterRpc(index, success, RpcTarget.Single(senderId,RpcTargetUse.Temp));
            return;
        }

        [Rpc(SendTo.SpecifiedInParams)]
        public void ClientSpawnCharacterRpc(int index,bool success, RpcParams rpcParams)
        {
            if (success)
            {
                playerHandler.Ready(index);
            }
        }*/
        [Rpc(SendTo.Server)]
        public void SpawnServerRpc(NetworkBehaviourReference RefBNet, int[] characterList, ulong senderId)
        {
            if (!IsServer) return;
            characterDictionary.Initialize();
            RefBNet.TryGet(out playerHandler);
            if (playerHandler == null) { Debug.LogError($"CHARCTER SWAP  SYS | SERVER SIDED : CANNOT GET PLAYERHANDLER OUT OF {RefBNet}"); return; }
            AddRowCharacter(characterList);
            SendClientCharacterListRpc(characterList, RpcTarget.Single(senderId, RpcTargetUse.Temp));
            int index = 1;
            Debug.Log($"CHARCTER SWAP  SYS | SERVER SIDED : Receive RPC from sender {senderId} to spawn all the following character", this);
            foreach (PlayerSO characterSO in ListCurrentChara)
            {
                if (characterList.Length < index) return;
                var _Char = NetworkManager.SpawnManager.InstantiateAndSpawn(characterSO.CharacterInfoData.AssociatedSkin, senderId);
                Debug.Log($"CHARCTER SWAP  SYS | SERVER SIDED : {characterSO.CharacterInfoData.ChatacterName}", this);
                CharacterHandler charHandler = _Char.AddComponent<CharacterHandler>();
                charHandler.Init(playerHandler, index, characterSO, playerHandler.Input, playerHandler.layerData, playerHandler.animationData, senderId);
                _Char.transform.SetParent(playerHandler.gameObject.transform);
                SendClientReferenceRpc(_Char, index, RpcTarget.Single(senderId, RpcTargetUse.Temp));
                index++;
            }

        }
        [Rpc(SendTo.SpecifiedInParams)]
        void SendClientCharacterListRpc(int[] characterList, RpcParams rpcParams)
        {
            AddRowCharacter(characterList);
        }
        [Rpc(SendTo.SpecifiedInParams)]
        void SendClientReferenceRpc(NetworkObjectReference RefONet, int index, RpcParams rpcParams)
        {
            playerHandler.init(RefONet, index);
        }
        #endregion
    }
#endif
    public class CharacterSOController : MonoBehaviour
    {
        protected CharacterDictionary characterDictionary = new();
        private PlayerOffHandler playerHandler;
        public CharacterSelecterUI characterSelecterUI { get; private set; }
        public CharacterSO[] ListCurrentChara { get; protected set; } = new CharacterSO[4];
        bool Iniated = false;
        public int CharacterNumber { get; private set; } = 0;
        public void Awake()
        {
            characterDictionary.Initialize();
            playerHandler = GetComponent<PlayerOffHandler>();
            characterSelecterUI = GameObject.FindAnyObjectByType<CharacterSelecterUI>().GetComponent<CharacterSelecterUI>();
            /*var temp = GetCurentCharacter();*/
        }
        private void AddRowCharacter(int[] CharcterList)
        {
            for (int i = 0; i < CharcterList.Length; i++)
            {
                AddCharacterToList(CharcterList[i], i);
            }
        }

        public int[] GetListCurrentCharacter()
        {
            int[] t = { 1 };
            return t;
        }
        private (bool, CharacterSO) ChekDuplicate(CharacterSO playerSO, CharacterSO[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == playerSO)
                {
                    return (true, null);
                }
            }
            return (false, playerSO);
        }
        private void AddCharacterToList(int id, int position)
        {
            try
            {
                CharacterSO temp = characterDictionary.GetPlayerSOFromId(id);
                if (ChekDuplicate(temp, ListCurrentChara) != (true, null))
                {
                    CharacterNumber++;
                    ListCurrentChara[position] = temp;
                    Debug.Log($"CHARCTER SWAP SYS: Added {temp.CharacterInfoData.ChatacterName} to position {position}", this);
                }
                else
                {
                    Debug.LogWarning($"CHARCTER SWAP SYS: Duplicate character ({temp.CharacterInfoData.ChatacterName} at position {position}), this character is not added", this);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"CHARCTER SWAP SYS: {e}", this);
            }
        }
        public void Spawn()
        {
            AddRowCharacter(GetListCurrentCharacter());
            int index = 0;
            for (int i = 0; i < CharacterNumber; i++)
            {
                if (index >= CharacterNumber) return;
                CharacterSO characterSO = ListCurrentChara[i];
                playerHandler.currentCharacters[i] = GameObject.Instantiate(characterSO.CharacterInfoData.AssociatedSkin,playerHandler.gameObject.transform);
                Debug.Log($"CHARCTER SWAP  SYS : {characterSO.CharacterInfoData.ChatacterName}", this);
                CharacterOffHandler charHandler = playerHandler.currentCharacters[i].AddComponent<CharacterOffHandler>();
                charHandler.Init(playerHandler, index, characterSO, playerHandler.Input, playerHandler.layerData, playerHandler.animationData);
                playerHandler.currentCharacters[i].transform.SetParent(playerHandler.gameObject.transform);

                index++;
            }
            characterSelecterUI.InitLightUI(ListCurrentChara, CharacterNumber);
            playerHandler.Spawn();
        }
    }
}
