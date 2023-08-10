using System;
using Sirenix.OdinInspector;
using Slime.Data.Constants;
using Slime.Data.Enums;
using Slime.Data.Skills;
using UnityEngine;
using UnityEngine.Serialization;
using Utils.Extensions;

namespace Slime.Data.Characters
{
    [Serializable]
    public class CharacterSkillData : SkillData
    {
        // public
        
        public override string Description =>
            base.Description.Resolve(
                Colors.HIGHLIGHT,
                Mathf.RoundToInt(Probability * 100),
                Mathf.RoundToInt(ActiveValue * 100),
                Mathf.RoundToInt(PassiveValue * 100),
                Cooldown);

        public override int Level => _unlocksAtCharacterLevel;
        public override float ActiveValue => _baseActiveValue;
        public override float PassiveValue => _basePassiveValue;
        public ETarget Target => _target;
        public ESkillType Type => _type;
        public EStatusEffect Effect => _effect;
        public float Probability => _probability;
        public string GetSpriteId() => $"{ID}";

        // private
        
        [Header("Character skill")]
        [SerializeField] private int _unlocksAtCharacterLevel;
        [SerializeField] private ETarget _target;
        [SerializeField] private ESkillType _type;
        [SerializeField, ShowIf("_type", ESkillType.StatusEffect)] private EStatusEffect _effect;
        [FormerlySerializedAs("_parameter")] [SerializeField, ShowIf("_type", ESkillType.ParameterChange)] private EAttribute _attribute;
        [SerializeField, Range(0, 1)] private float _probability;
    }
}