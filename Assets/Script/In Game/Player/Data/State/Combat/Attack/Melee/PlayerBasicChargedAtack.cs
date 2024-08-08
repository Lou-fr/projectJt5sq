using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerBasicChargedAtack 
    {
        [field: Header("Base charge attack stats")]
        [field: SerializeField][field: Range(0f, 5f)] public float TimeBitwinChargeAttack { get; private set; } = 1f;
        [field: SerializeField][field: Range(20, 180)] public int BaseChargeAttackDamage { get; private set; } = 30;
    }
}
