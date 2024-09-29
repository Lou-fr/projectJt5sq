using UnityEngine;

namespace BleizEntertainment.Maps.death
{
    public class DeathSystem : MonoBehaviour
    {
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
            Vector3 nearestPos = new Vector3(-666,-666,-666);
            Vector3[] regionWrapPos = WrapMain.AllWrapAtCurentRegion(regionId);
            foreach(Vector3 WrapPos in regionWrapPos)
            {
                Debug.Log(Vector3.Distance(playerDeathPos, nearestPos) + " | " + Vector3.Distance(playerDeathPos, WrapPos));
                if(Vector3.Distance(playerDeathPos, nearestPos) > Vector3.Distance(playerDeathPos, WrapPos))
                {
                    nearestPos = WrapPos;
                }
            }

        }
    }
}
