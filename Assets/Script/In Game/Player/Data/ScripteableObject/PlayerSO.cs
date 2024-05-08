using UnityEngine;

namespace BleizEntertainment
{
    [CreateAssetMenu(fileName ="Player",menuName ="BleizEntertainment/Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field :SerializeField]public PlayerGroundedData groundedData { get; private set; }
        [field :SerializeField]public PlayerAirborneData airboneData { get; private set; }
    }
}