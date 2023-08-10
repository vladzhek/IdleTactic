using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.Data.IDs;
using Slime.Data.IDs.Boosters;
using UnityEngine;

namespace Slime.Data.Boosters
{
    [Serializable]
    public class BoosterData
    {
        [ValueDropdown(nameof(IDs))] public string ID;
        public Sprite Sprite;
        public Color ColorBG;
        public string Title;
        public string Description;
        
        [Range(0,4)]public float StartingValue;
        [Range(0,4)]public float IncreaseValueForLevel;

        public int Duration;
        public int ForNextUpgrade;
        public int MaxLevel;

        private IEnumerable<string> IDs => BoostersIDs.Values;
    }
}