using BleizEntertainment.Maps;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BleizEntertainment
{
    public class Warp : MonoBehaviour
    {
        [SerializeField] int regionId;
        [SerializeField] int warpId;
        Button warp;
        public static Action<Vector3> MapsWarp = delegate { }; 
        private void Start()
        {
            warp = GetComponent<Button>();
            warp.onClick.AddListener(OnClick);
        }
        async void OnClick()
        {
            WrapMain wrap = GameObject.FindFirstObjectByType<WrapMain>().GetComponent<WrapMain>();
            Vector3 WarpOnWorld = await wrap.wrapToWrap(regionId, warpId);
            if (WarpOnWorld == new Vector3(-1, -1, -1))
            {
                Debug.LogError("Warp doesn't exist please verify map config");
                return;
            }
            await wrap.Preload(regionId, WarpOnWorld);
            MapsWarp.Invoke(WarpOnWorld);
        }
    }
}
