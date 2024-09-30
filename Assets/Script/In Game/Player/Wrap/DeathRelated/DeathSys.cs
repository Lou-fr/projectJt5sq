using System;
using System.Collections.Generic;
using UnityEngine;

namespace BleizEntertainment.Maps.death
{
    public class DeathSystem : MonoBehaviour
    {
        public static Action<int,bool,bool> healAndRespawn = delegate { };
        public static Action<Vector3> wrapAndRespawn = delegate { };
        public void Start()
        {
            PlayerOffHandler.allDead += NearestSpawnPoint;
        }
        private void OnDestroy()
        {
            PlayerOffHandler.allDead -= NearestSpawnPoint;
        }
        void NearestSpawnPoint(int regionId, Vector3 playerDeathPos)
        {
            Vector3 nearestPos = new Vector3(-666, -666, -666);
            foreach (Vector3 WrapPos in FilterdWrap(regionId))
            {
                Debug.Log(Vector3.Distance(playerDeathPos, nearestPos) + " | " + Vector3.Distance(playerDeathPos, WrapPos));
                if (Vector3.Distance(playerDeathPos, nearestPos) > Vector3.Distance(playerDeathPos, WrapPos))
                {
                    nearestPos = WrapPos;
                }
            }
            wrapAndRespawn?.Invoke(nearestPos);
            healAndRespawn?.Invoke(-1,true,true);

        }
        Vector3[] FilterdWrap(int regionId)
        {
            List<Vector3> filteredWrap = new List<Vector3>();
            foreach (WrapDict wrapDict in WrapMain.AllWrapAtCurentRegion(regionId))
            {

                if (wrapDict.CanHeal)
                {
                    filteredWrap.Add(wrapDict.Wrap);
                }
            }
            return filteredWrap.ToArray();
        }
    }
}
