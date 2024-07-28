using UnityEngine;

namespace BleizEntertainment
{
    public class CapsuleColliderData
    {
        public CapsuleCollider collider { get; private set; }
        public string Id { get; private set; }
        public Vector3 ColliderCenterInLocalSpace { get; private set; }
        public Vector3 ColliderVerticalExtents { get; private set; }

        public void Initialize(GameObject gameObject)
        {
            if (collider != null) return;
            Id = gameObject.name;
            collider = gameObject.GetComponent<CapsuleCollider>();
            UpdateColliderData();
        }

        public void UpdateColliderData()
        {
            ColliderCenterInLocalSpace = collider.center;
            ColliderVerticalExtents = new Vector3(0f, collider.bounds.extents.y, 0f);
        }
    }
}
