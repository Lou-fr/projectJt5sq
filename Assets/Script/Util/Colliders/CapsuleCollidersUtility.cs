using System;
using UnityEngine;

namespace BleizEntertainment
{
    [Serializable]
    public class CapsuleCollidersUtility
    {
        public CapsuleColliderData capsuleColliderData { get; private set; }
        [field: SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
        [field: SerializeField] public SlopeData slopeData { get; private set; }

        public void Initialize(GameObject gameObject)
        {
            if (capsuleColliderData != null) if (capsuleColliderData.Id == gameObject.name) return;
            capsuleColliderData = new CapsuleColliderData();
            capsuleColliderData.Initialize(gameObject);
        }

        public void CalculateCapsuleColliderDimension()
        {
            SetCapsuleColliderRadius(DefaultColliderData.Radius);
            SetCapsuleCollideHeight(DefaultColliderData.Height * (1f - slopeData.StepHeightPercentage));
            RecalculateCapsuleColliderCenter();
            float halfColliderHeight = capsuleColliderData.collider.height;
            if (halfColliderHeight / 2f < capsuleColliderData.collider.radius)
            {
                SetCapsuleColliderRadius(halfColliderHeight);
            }
            capsuleColliderData.UpdateColliderData();
        }



        public void SetCapsuleColliderRadius(float radius)
        {
            capsuleColliderData.collider.radius = radius;
        }
        public void SetCapsuleCollideHeight(float height)
        {
            capsuleColliderData.collider.height = height;
        }
        public void RecalculateCapsuleColliderCenter()
        {
            float colliderHeightDifference = DefaultColliderData.Height - capsuleColliderData.collider.height;
            Vector3 newColliderCenter = new Vector3(0f, DefaultColliderData.CenterY + (colliderHeightDifference / 2f), 0f);
            capsuleColliderData.collider.center = newColliderCenter;
        }
    }
}
