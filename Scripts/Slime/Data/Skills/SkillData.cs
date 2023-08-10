using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.Data.Abstract;
using Slime.Data.Constants;
using Slime.Data.IDs;
using UnityEngine;
using Utils.Extensions;

namespace Slime.Data.Skills
{
    [Serializable]
    public class SkillData : EntityData, ISummonable
    {
        // public
        
        public override string ID { 
            get => _skillID;
            set => _skillID = value;
        }
        
        public override string Description =>
            base.Description.Resolve(
                Colors.HIGHLIGHT,
               // Mathf.RoundToInt(Probability * 100),
                0,
                Mathf.RoundToInt(ActiveValue * 100),
                Mathf.RoundToInt(PassiveValue * 100),
                Cooldown);
        
        public float Cooldown => _cooldown;
        
        public override string ToString()
        {
            return $"{base.ToString()} id: {ID}";
        }

        // private
        
        [Header("Skill")]
        [SerializeField, ValueDropdown(nameof(IDs))] private string _skillID;
        [SerializeField] private float _cooldown;
        
        // ReSharper disable once InconsistentNaming
        private IEnumerable<string> IDs => SkillIDs.Values;
    }
}