using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class PlayerTriggerColliderData
    {
        [field: SerializeField] public BoxCollider GroundCheckCollider { get; private set; }
        public Vector3 GroundCheckColliderVerticalExtents { get; private set; }

        public void Initialize()
        {
            GroundCheckColliderVerticalExtents = GroundCheckCollider.bounds.extents;
        }
    }
}
