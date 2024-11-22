using System;
using UnityEngine;

namespace BleizEntertainment.Maps
{
    [Serializable]
    public class MapsWrapDict
    {
        [field: SerializeField]
        public int Id { get; internal set; }
        [field: SerializeField]
        public Vector2 MapsWrap { get; internal set; }
        [field: SerializeField]
        [field :Header("Display in the In Game maps")]
        public string WrapName { get; internal set; }
        public MapsWrapDict(int id,Vector3 Wrap,string wrapName)
        {
            Id = id;
            MapsWrap = new Vector2(Wrap.x, Wrap.z); 
            WrapName = wrapName;
        }
    }
}
