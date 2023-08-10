using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.Data.Abilities;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.Data.IDs;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Slime.Data.Characters
{
    [Serializable]
    public class CharacterData : EntityData, ISummonable
    {
        public override string ID => _characterID;
        public EGrade Grade => _grade;
        public IEnumerable<AbilityData> Abilities { get; set; }
        public CharacterSkillData Skill { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()} id: {ID}";
        }
        
        [Header("Character")]
        [SerializeField, ValueDropdown(nameof(IDs))] private string _characterID;
        [SerializeField] private EGrade _grade;

        private IEnumerable<string> IDs => CharacterIDs.Values;
    }
}