using System;
using System.Collections.Generic;
using System.Linq;

namespace Slime.Data.Enums
{
    public enum EStageType
    {
        Default,
        BossRush,
        GoldRush,
    }

    public static class EStageExtensions
    {
        public static readonly IEnumerable<EStageType> Values =
            from EStageType type in Enum.GetValues(typeof(EStageType))
            select type;

        public static string ToTitle(this EStageType type)
        {
            return type switch
            {
                // NOTE: move to colors ?
                EStageType.BossRush => $"boss rush",
                EStageType.GoldRush =>  $"gold rush",
                _ => ""
            };
        }
    }
}