using System;
using System.Collections.Generic;
using System.Linq;
using Slime.Data.IDs;
using Utils.Extensions;

namespace Slime.Data.Enums
{
    public enum EAttribute
    {
        Damage,
        Health,
        Regeneration,
        RegenerationRate,
        AttackSpeed,
        CriticalChance,
        CriticalDamage,
        DoubleAttackChance,
        TripleAttackChance,
        
        MovementSpeed,
        EvasionChance,
        SkillCooldown,
        AttackRadius
    }
    
    public static class EAttributeExtensions {
        public static readonly IEnumerable<EAttribute> Values = 
            from EAttribute type in Enum.GetValues(typeof(EAttribute)) 
            select type;

        public static string ToID(this EAttribute type)
        {
            return type switch
            {
                EAttribute.Damage => AttributeIDs.Damage,
                EAttribute.Health => AttributeIDs.Health,
                EAttribute.Regeneration => AttributeIDs.Regeneration,
                EAttribute.RegenerationRate => AttributeIDs.RegenerationRate,
                EAttribute.AttackSpeed => AttributeIDs.AttackSpeed,
                EAttribute.CriticalChance => AttributeIDs.CriticalChance,
                EAttribute.CriticalDamage => AttributeIDs.CriticalDamage,
                EAttribute.DoubleAttackChance => AttributeIDs.DoubleAttackChance,
                EAttribute.TripleAttackChance => AttributeIDs.TripleAttackChance,
                EAttribute.AttackRadius => AttributeIDs.AttackRadius,
                EAttribute.MovementSpeed => AttributeIDs.MoveSpeed,
                _ => type.ToString().Uncapitalize()
            };
        }
    }
}