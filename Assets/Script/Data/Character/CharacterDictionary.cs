using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    public class CharacterDictionary
    {
        public IDictionary<int, CharacterSO> CharacterDicti { get; private set; } = new Dictionary<int, CharacterSO>(); 

        public CharacterSO GetPlayerSOFromId(int id)
        {
            return CharacterDicti[id];
        }
        public void Initialize()
        {
            CharacterSO[] load = Resources.LoadAll<CharacterSO>("Character/");
            foreach (CharacterSO characterInfo in load)
            {
                CharacterDicti.Add(characterInfo.CharacterInfoData.ChatacterId, characterInfo);
                Debug.Log($"CHARCTER DICITIONNARY : Added {characterInfo.CharacterInfoData.ChatacterName} at the id {characterInfo.CharacterInfoData.ChatacterId}");    
            }
        }
    }
}
