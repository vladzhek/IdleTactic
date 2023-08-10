using System;
using Slime.Data.Parameters;
using UnityEngine;

namespace Slime.Data.Abilities
{
    [Serializable]
    public class AbilityData
    {
        // public 

        public Sprite Sprite { get; set; }
        public int UnlocksAtCharacterLevel => _unlocksAtCharacterLevel;
        public ParameterData Parameter => _parameterData;

        public string GetSpriteId() => $"ability{Parameter.Type}";

        // private
        
        [Header("Ability")]
        [SerializeField] private int _unlocksAtCharacterLevel;
        [SerializeField] private ParameterData _parameterData;
    }
}