using UnityEngine;

namespace BleizEntertainment
{
    [CreateAssetMenu(fileName = "Player", menuName = "BleizEntertainment/Characters/Player")]
    public class CharacterSO : ScriptableObject
    {
        [field: SerializeField] public ChatacterInfoData CharacterInfoData { get; private set; }
        [field: SerializeField] public PlayerGroundedData groundedData { get; private set; }
        [field: SerializeField] public PlayerAirborneData airboneData { get; private set; }
        [field: SerializeField] public PlayerCombatDataState combatData { get; private set; }
    }
}