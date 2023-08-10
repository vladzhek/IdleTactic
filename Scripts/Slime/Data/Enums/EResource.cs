using System;
using System.Collections.Generic;
using System.Linq;

namespace Slime.Data.Enums
{
    public enum EResource
    {
        RealCurrency,

        SoftCurrency,
        HardCurrency,
        GoldRushCurrency,
        BossRushCurrency,
        
        Character,
        CharacterUpgradeCurrency
    }
    
    public static class EResourceExtensions {
        public static readonly IEnumerable<EResource> Values = 
            from EResource type in Enum.GetValues(typeof(EResource)) 
            select type;

        public static float GetInitialQuantity(this EResource type)
        {
            return type switch
            {
                EResource.SoftCurrency => Constants.Values.SOFT_CURRENCY_INITIAL_QUANTITY,
                EResource.CharacterUpgradeCurrency => Constants.Values.CHARACTER_UPGRADE_CURRENCY_INITIAL_QUANTITY,
                EResource.HardCurrency => Constants.Values.HARD_CURRENCY_INITIAL_QUANTITY,
                EResource.BossRushCurrency => Constants.Values.BOSS_RUSH_CURRENCY_INITIAL_QUANTITY,
                EResource.GoldRushCurrency => Constants.Values.GOLD_RUSH_CURRENCY_INITIAL_QUANTITY,
                _ => 0
            };
        }
        
        public static string ToSpriteID(this EResource type)
        {
            return type switch
            {
                _ => $"resource{type}"
            };
        }
    }
}