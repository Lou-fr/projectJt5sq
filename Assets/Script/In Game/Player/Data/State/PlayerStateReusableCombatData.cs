namespace BleizEntertainment
{
    public class PlayerStateReusableCombatData
    {
        public float BasicAtackEnterTime { get; set; } = 0f;
        public int currentConsecutiveAttack { get; set; } = 0;
        public float LastAtackEnterTime { get; set; } = 0f;
        public float BasicChargeAttackEnterTime { get; set; } = 0f;
    }
}
