using System.Collections.Generic;
using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs
{
    // TODO: remove
    public abstract class AttributeIDs : IDList<AttributeIDs>
    {
        public new static string Default = Health;
        
        // common
        public const string Health = "health";
        public const string Damage = "damage";
        public const string AttackRadius = "attackRadius";
        public const string AttackSpeed = "attackSpeed";

        // player
        public const string CriticalChance = "criticalChance";
        public const string CriticalDamage = "criticalDamage";
        public const string Regeneration = "regeneration";
        public const string RegenerationRate = "regenerationRate";
        public const string DoubleAttackChance = "doubleAttackChance";
        public const string TripleAttackChance = "tripleAttackChance";

        // enemies
        public const string MoveSpeed = "moveSpeed";
        
        // Companion 
        public const string FollowDistance = "FollowDistance";

        public static IEnumerable<string> Upgradable = new[]
        {
            Health,
            Damage,
            AttackRadius,
            AttackSpeed,
            CriticalChance,
            CriticalDamage,
            Regeneration,
            RegenerationRate,
            DoubleAttackChance,
            TripleAttackChance,
            FollowDistance,
        };
    }
}