using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerCapsuleCollidersUtilities 
    {
        [field:SerializeField]public CapsuleCollidersUtility CapsuleCollidersUtility { get;private set; }
        [field:SerializeField]public PlayerTriggerColliderData TriggerColliderData { get;private set; }
    }
}
