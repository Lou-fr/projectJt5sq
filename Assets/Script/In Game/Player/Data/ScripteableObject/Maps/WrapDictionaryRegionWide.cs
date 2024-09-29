using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment.Maps
{
    [CreateAssetMenu(fileName = "WrapRegion", menuName = "BleizEntertainment/Maps/Wrap")]
    public class WrapDictionaryRegionWide:ScriptableObject
    {
        [field: Header("Region Info")]
        [field: SerializeField] public string RegionName {  get; private set; }
        [field: SerializeField] public int RegionId { get; private set; }
        [field: Header("Wraps Info")]
        [field: SerializeField] public WrapDict[] Wraps { get; private set; }
    }
}
