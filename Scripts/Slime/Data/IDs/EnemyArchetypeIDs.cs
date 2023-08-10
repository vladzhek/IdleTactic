using JetBrains.Annotations;
using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs
{
    public abstract class EnemyArchetypeIDs : IDList<EnemyArchetypeIDs>
    {
        public const string UNIT_1 = "unit1";
        public const string UNIT_2 = "unit2";
        public const string UNIT_3 = "unit3";
        public const string UNIT_4 = "unit4";
        public const string BOSS = "boss";
        public const string GOLD_RUSH = "goldRush";
        
        /*
        // Attack type + HP
        public const string SMALL_MELEE = "meleeSlow";
        public const string BIG_MELEE = "meleeSlow";
        public const string SMALL_RANGE = "meleeSlow";
        public const string BIG_RANGE = "meleeSlow";
        public const string BOSS_MELEE = "meleeSlow";
        public const string BOSS_RANGE = "meleeSlow";
        
        // Class
        public const string Warrior = "meleeSlow";
        public const string Archer = "meleeSlow";
        public const string Mage = "meleeSlow";
        public const string Boss = "meleeSlow";
        
        // All types
        public const string MeleeFast = "meleeFast";
        public const string Range = "range";
        public const string BossMelee = "bossMelee";
        public const string BossRange = "bossRange";
        
        // attack type = melee, ranged
        // attack speed = fast, slow, normal
        // attack type = light, medium, high, ultimate
        // unit size = small, medium, big, boss 
        // unit health = low, medium, high 
        */
        
    }
}