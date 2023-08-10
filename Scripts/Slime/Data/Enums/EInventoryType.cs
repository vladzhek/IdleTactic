using System;
using System.Collections.Generic;
using System.Linq;

namespace Slime.Data.Enums
{
    public enum EInventoryType
    {
        Weapon,
        Armor,
        Accessory
    }
    
    public static class EInventoryTypeExtensions {
        public static IEnumerable<EInventoryType> Values => 
            from EInventoryType type in Enum.GetValues(typeof(EInventoryType)) 
            select type;
        
        public static string GetTitle(this EInventoryType type)
        {
            return type switch
            {
                // NOTE: move to strings?
                EInventoryType.Accessory => "System",
                _ => type.ToString()
            };
        }
    }
}