using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class ChatacterInfoData
    {
        [field: Header("Character Information")]
        [field: SerializeField] public string ChatacterName { get; private set; }
        [field: SerializeField] public int ChatacterId { get; private set; }

    }
}
