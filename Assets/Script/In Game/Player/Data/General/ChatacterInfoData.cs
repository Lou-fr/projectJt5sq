using System;
using Unity.Netcode;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class ChatacterInfoData
    {
        [field: Header("Character Information")]
        [field: SerializeField] public string ChatacterName { get; private set; }
        [field: SerializeField] public int ChatacterId { get; private set; }
        [field: SerializeField, Range(0,500)] public int BaseCharacterHp { get; private set; }
        [field: Header("In-game visual Character Information")]
        [field: SerializeField] public RuntimeAnimatorController Controller { get; private set; }
        [field: SerializeField] public GameObject AssociatedSkin {  get; private set; }
        [field: Header("Character calculation Information")]
        [field: SerializeField] public float characterHeight { get; private set; }
    }
}
