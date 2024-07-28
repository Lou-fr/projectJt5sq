using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    public class CharacterDictionary
    {
        public IDictionary<int, PlayerSO> CharacterDicti { get; private set; } = new Dictionary<int, PlayerSO>(); 

        public PlayerSO GetPlayerSOFromId(int id)
        {
            return CharacterDicti[id];
        }
        public void Initialize()
        {
            PlayerSO[] load = Resources.LoadAll<PlayerSO>("Character/");
            foreach (PlayerSO characterInfo in load)
            {
                    CharacterDicti.Add(characterInfo.CharacterInfoData.ChatacterId, characterInfo);
                Debug.Log($"CHARCTER DICITIONNARY : Added {characterInfo.CharacterInfoData.ChatacterName} at the id {characterInfo.CharacterInfoData.ChatacterId}");    
            }
        }
    }
}
