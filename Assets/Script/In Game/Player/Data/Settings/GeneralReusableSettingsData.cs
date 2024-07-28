using System;
using UnityEngine;

namespace BleizEntertainment
{
    public class GeneralReusableSettingsData
    {
        public float UserZoomSensivity { get; set; } = 1.0f;
    }
    [Serializable]
    public class GeneralSettingsData 
    { 
        [field: SerializeField, Range(0.1f,10f)] public float BaseZoomSensivity { get; private set; } = 1.0f;
    }
}