using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Configs.Skills;
using Slime.Data.Progress.Abstract;
using Slime.Data.Skills;
using Slime.Models.Abstract;
using Utils;
using SkillConfig = Slime.Configs.Skills.SkillConfig;

namespace Slime.Models
{
    [UsedImplicitly]
    public class SkillsModel : BaseSummonnableModel<SkillsConfig, SkillConfig, SkillData, int>, ISkillsModel
    {
        // NOTE: event hides existing
        // NOTE: use one event
        public event Action<int, string> OnEquip;
        public event Action<int, string> OnRemove;

        private int _selectedIndex;
        
        private SkillsModel(IGameProgressModel progressModel, ISpritesModel spritesModel)
            : base(progressModel, spritesModel)
        {
        }

        public bool IsLoaded => ProgressModel.IsLoaded;


        #region BaseSummonableModel implementation

        protected override string ConfigPath => "Skills";

        protected override Dictionary<string, ProgressData> GetProgressData()
        {
            var data = GameData.SkillsData;
            if (data == null)
            {
                data = new Dictionary<string, ProgressData>();
                GameData.SkillsData = data;
            }

            return data;
        }

        protected override Dictionary<int, string> GetEquippedData()
        {
            var data = GameData.EquippedSkillsData;
            if (data == null)
            {
                data = new Dictionary<int, string>();
                GameData.EquippedSkillsData = data;
            }

            return data;
        }

        protected override int GetEquipmentKey(SkillData data)
        {
            return _selectedIndex;
        }

        public void Equip(string id, int index, bool shouldEquip = true)
        {
            var isAlreadyEquipped = IsEquipped(id);
            if (isAlreadyEquipped == shouldEquip)
            {
                Logger.Warning($"equipment {id} is already {(shouldEquip ? "equipped" : "unequipped")}");
                return;
            }

            _selectedIndex = index;
            base.Equip(id, shouldEquip);

            if (shouldEquip) OnEquip?.Invoke(index, id);
            else OnRemove?.Invoke(index, id);
        }
        protected override bool ShouldUseActiveValueInParameterCalculations => false;

        #endregion

        public float GetTotalPassiveEffect()
        {
            return Get().Where(x => x.IsUnlocked).Sum(data => data.PassiveValue);
        }
    }
}