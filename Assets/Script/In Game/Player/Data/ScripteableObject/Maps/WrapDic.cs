using System;
using UnityEngine;

namespace BleizEntertainment.Maps
{
    [Serializable]
    public class WrapDict
    {
        [field: SerializeField]
        public int Id { get; private set; }
        [field: SerializeField]
        public Vector3 Wrap { get; private set; }
        [field: SerializeField]
        public bool CanHeal { get; private set; }
        [field: SerializeField]
        [field :Header("Display in the In Game maps")]
        public string WrapName { get; private set; }
    }
}
