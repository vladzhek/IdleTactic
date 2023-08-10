using System;
using System.Collections.Generic;
using System.Linq;
using Slime.Data.Constants;
using UnityEngine;
using Utils.Extensions;

namespace Slime.Data.Enums
{
    public enum ERarity
    {
        Common,
        Uncommon,
        Rare,
        Elite,
        Exotic,
        Heroic,
        Epic,
        Legendary,
        Mystic,
        Divine,
        Transcendent
    }
    
    public static class ERarityExtensions {
        public static readonly IEnumerable<ERarity> Values = 
            from ERarity type in Enum.GetValues(typeof(ERarity)) 
            select type;

        public static readonly IEnumerable<ERarity> Implemented = 
            from item in Values
            where IsImplemented(item)
            select item;

        public static bool IsImplemented(ERarity item) => !Unimplemented.Contains(item);
        
        public static Color ToColor(this ERarity type)
        {
            return type switch
            {
                // NOTE: move to colors ?
                ERarity.Common => ColorExtensions.FromHex("B3B3B3"),
                ERarity.Uncommon => ColorExtensions.FromHex("4CAF50"),
                ERarity.Rare => ColorExtensions.FromHex("5F9EA0"),
                ERarity.Elite => ColorExtensions.FromHex("E15F5F"),
                ERarity.Exotic => ColorExtensions.FromHex("FF5722"),
                ERarity.Heroic => ColorExtensions.FromHex("8B0000"),
                ERarity.Epic => ColorExtensions.FromHex("6A0DAD"),
                ERarity.Legendary => ColorExtensions.FromHex("9C27B0"),
                ERarity.Mystic => ColorExtensions.FromHex("ff0597"),
                ERarity.Divine => ColorExtensions.FromHex("00BCD4"),
                ERarity.Transcendent => ColorExtensions.FromHex("FFD700"),
                _ => Colors.LIGHT_GRAY
            };
        }
        
        private static readonly IEnumerable<ERarity> Unimplemented = new[]
        {
            ERarity.Heroic,
            ERarity.Epic,
            ERarity.Legendary,
            ERarity.Mystic,
            ERarity.Divine,
            ERarity.Transcendent
        };
    }
}