using UnityEngine;

namespace BleizEntertainment.Maps
{
    [CreateAssetMenu(fileName = "WrapRegion", menuName = "BleizEntertainment/Maps/Wrap")]
    public class WrapDictionaryRegionWide : ScriptableObject
    {
        [field: Header("Region Info")]
        [field: Tooltip("Please add a warp maps with same name and ID")]
        [field: SerializeField] public string RegionName { get; private set; }
        [field: SerializeField] public int RegionId { get; private set; }
        [field: Header("Wraps Info")]
        [field: SerializeField] public WrapDict[] Wraps { get; private set; }
        public MapsWrapDict[] MapsWraps { get; private set; }
        private void OnValidate()
        {
            //add automatic maps wrap and ID increment
            int i = 0;
            MapsWraps = new MapsWrapDict[Wraps.Length];
            foreach (var wrp in Wraps)
            {
                wrp.Id = i;
                MapsWraps[i] = new MapsWrapDict(wrp.Id, wrp.Wrap, wrp.WrapName);
                i++;
            }
        }
    }

}
