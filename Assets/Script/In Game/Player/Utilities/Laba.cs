using UnityEngine;

namespace BleizEntertainment
{
    public class Laba : MonoBehaviour
    {
        [SerializeField]MeshCollider MeshCollider;
        [SerializeField] int timeSinceIn = 0;
        // THIS SCRIPT IS FOR TEST PURPOSE ONLY
        void Start()
        {
            MeshCollider = GetComponent<MeshCollider>();
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.tag, this);
            if (other.tag != "Player")return;
            PlayerOffHandler.hitTaken(10,"laba");
            timeSinceIn = 1;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.tag != "Player") return;
            PlayerOffHandler.hitTaken(10*timeSinceIn, "laba");
            timeSinceIn++;
        }
        private void OnTriggerExit(Collider other)
        {
            timeSinceIn = 0;
        }
    }
}
