using Slime.Data.Enums;
using UnityEngine;

namespace Slime.Data.Abstract
{
    public interface IDisplayable : IData
    {
        public bool IsUnlocked { get; }
        public string Title { get; }
        public string Description { get; }
        public int Order { get; }
        public Sprite Sprite { get; set; }
        public ERarity Rarity { get; }
        public float ActiveValue { get; }
        public float PassiveValue { get; }
    }
}