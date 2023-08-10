using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs
{
    public abstract class EffectIDs : IDList<EffectIDs>
    {
        public const string PLAYER_ATTACK = "playerAttack";
        public const string GOLD = "gold";
        public const string AOE_MISSILE = "aoeMissile";
        public const string LASER = "laser";
        public const string ATTACK_SPEED_BOOST = "attackSpeedBoost";
        public const string SKILL_4 = "skill4";
        public const string SKILL_5 = "skill5";
        public const string SKILL_6 = "skill6";
    }
}