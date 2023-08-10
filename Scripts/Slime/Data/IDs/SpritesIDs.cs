using Slime.Data.Enums;
using Slime.Data.IDs.Abstract;
// ReSharper disable InconsistentNaming

namespace Slime.Data.IDs
{
    public abstract class SpritesIDs : IDList<SpritesIDs>
    {
        public new static string Default => NO_IMAGE;
        
        /* COMMON */
        public const string NO_IMAGE = "noImage";
        public const string ADS_ICON = "adsIcon";

        public const string AUTO_SKILL_SLOT = "autoSkillSlot";
        public const string EMPTY_SKILL_SLOT = "emptySkillSlot";
        
        /* FOOTER */
        // footer tabbar
        public const string CHARACTER_FOOTER_TAB_ICON = "characterFooterTabIcon";
        public const string COMPANIONS_FOOTER_TAB_ICON = "companionsFooterTabIcon";
        public const string DUNGEONS_FOOTER_TAB_ICON = "dungeonsFooterTabIcon";
        public const string CASTLE_FOOTER_TAB_ICON = "castleFooterTabIcon";
        public const string STORE_FOOTER_TAB_ICON = "storeFooterTabIcon";
        public const string LOCKED_FOOTER_TAB_ICON = "lockedFooterTabIcon";
        
        /* CHARACTER */
        // equipment
        public static readonly string WEAPON = "default" + EInventoryType.Weapon;
        public static readonly string ARMOR = "default" + EInventoryType.Armor;
        public static readonly string ACCESSORY = "default" + EInventoryType.Accessory;

        // weapons
        public const string BIG_BARREL = "weaponBigBarrel";
        public const string ROCKET_LAUNCHER = "weaponRocketLauncher";
        public const string MODIFIED_CANNON = "weaponModifiedCannon";
        public const string RAIL_GUN = "weaponRailgun";
        public const string BOMBARD = "weaponBombard";
        
        // armor
        public const string KEVLAR_ARMOR = "armorKevlar";
        public const string ELECTRIC_PLATES = "armorElectricPlates";
        public const string METEOR_ARMOR = "armorMeteor";
        public const string PLASMA_SHIELD = "armorPlasmaShield";
        public const string NANOPARTICLES = "armorNanoparticles";
        
        // accessories
        public const string LASER_POINTER = "accessoryLaserPointer";
        public const string IMPROVED_MECHANISM = "accessoryImprovedMechanism";
        public const string ADDITIONAL_AIMER = "accessoryAdditionalAimer";
        public const string EXPLOSIVE_PROJECTILE = "accessoryExplosiveProjectile";
        public const string MUZZLE = "accessoryMuzzle";

        // attributes
        public static readonly string ATTRIBUTE_DAMAGE = "attribute" + EAttribute.Damage;
        public static readonly string ATTRIBUTE_HEALTH = "attribute" + EAttribute.Health;
        public static readonly string ATTRIBUTE_REGENERATION = "attribute" + EAttribute.Regeneration;
        public static readonly string ATTRIBUTE_ATTACK_SPEED = "attribute" + EAttribute.AttackSpeed;
        public static readonly string ATTRIBUTE_CRITICAL_CHANCE = "attribute" + EAttribute.CriticalChance;
        public static readonly string ATTRIBUTE_DOUBLE_CHANCE = "attribute" + EAttribute.DoubleAttackChance;
        public static readonly string ATTRIBUTE_TRIPLE_CHANCE = "attribute" + EAttribute.TripleAttackChance;
        
        // abilities
        public static readonly string ABILITY_DAMAGE = "ability" + EAttribute.Damage;
        public static readonly string ABILITY_HEALTH = "ability" + EAttribute.Health;
        //public static readonly string ABILITY_REGENERATION = "ability" + EAttribute.Regeneration;
        public static readonly string ABILITY_ATTACK_SPEED = "ability" + EAttribute.AttackSpeed;
        //CriticalChance,
        //CriticalDamage,
        //DoubleAttackChance,
        //TripleAttackChance,
        public static readonly string ABILITY_COOLDOWN = "ability" + EAttribute.SkillCooldown;
        
        // character tabbar
        public const string CHARACTER_ICON = "characterIcon";
        public const string SKILL_ICON = "skillIcon";
        
        /* STORE */
        // store tabbar
        public const string SUMMON_ICON = "summonIcon";
        public const string SHOP_ICON = "shopIcon";

        // Companions
        public const string LITTLE_TANK = "LittleTank";
        public const string ARTILLERY = "Artillery";
        public const string FLYING_DRONE = "FlyingDrone";
        public const string SMALL_HELICOPTER = "SmallHelicopter";
        public const string TANK_DOUBLE_BARREL = "TankDoubleBarrel";
    }
}