using BleizEntertainment.Maps;
using BleizEntertainment.Maps.death;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment
{
    [RequireComponent(typeof(DeathSystem))]
    public class WrapMain : MonoBehaviour
    {
        static IDictionary<int, WrapDictionaryRegionWide> WrapDictionaryMapsWide  = new Dictionary<int, WrapDictionaryRegionWide>();
        void Start()
        {
            foreach (WrapDictionaryRegionWide wrapRegion in Resources.LoadAll<WrapDictionaryRegionWide>("Maps/WrapRegion/"))
            {
                WrapDictionaryMapsWide.Add(wrapRegion.RegionId, wrapRegion);
                Debug.Log($"WRAP SYSTEM | Added {wrapRegion.RegionName} at id {wrapRegion.RegionId} to the dictionary",this);
            }
        }
        #region Death Related function
        public static WrapDict[] AllWrapAtCurentRegion(int regionId)
        {
            return WrapDictionaryMapsWide[regionId].Wraps;
        }
        #endregion
        #region General Use Function
        public Vector3 wrapToWrap(int regionId,int wrapId )
        {
            WrapDict[] wraps = WrapDictionaryMapsWide[regionId].Wraps;
            foreach(WrapDict wrap in wraps)
            {
                if(wrap.Id == wrapId) return wrap.Wrap;
            }
            return new Vector3(-1, -1, -1);
        }
        #endregion
    }
}
