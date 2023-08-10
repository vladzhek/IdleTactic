using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sirenix.OdinInspector;
using Slime.Data.Enums;
using Slime.Data.IDs;
using UnityEngine;
using Utils.Extensions;

namespace Slime.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SpriteEntry
    {
        [ValueDropdown(nameof(IDs))] public string ID;
        [AssetSelector(Paths = "Assets")] public Sprite Sprite;

        private IEnumerable<string> IDs => SpritesIDs.Values
            .Concat(CharacterIDs.Values)
            .Concat(AttributeIDs.Values)
            .Concat(InventoryIDs.Values)
            .Concat(EnumValues<EResource>("resource"))
            .Concat(SkillIDs.Values)
            .Concat(SkillIDs.Values)
            .Concat(ProductIDs.ValuesWithPrefix("product"));

        // TODO: add for all relevant enums + prefix
		private IEnumerable<string> EnumValues<TEnum>(string prefix = null) where TEnum : Enum
        {
            return from value in Enum.GetNames(typeof(TEnum)) 
                select $"{prefix}{(string.IsNullOrEmpty(prefix) ? value.Uncapitalize() : value)}";
        }
    }
}