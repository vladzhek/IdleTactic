using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Configs.Companions;
using Slime.Data.Inventory;
using Slime.Data.Progress.Abstract;
using Slime.Models.Abstract;

namespace Slime.Models
{
    [UsedImplicitly]
    public class CompanionsModel : 
        BaseSummonnableModel<CompanionsConfig, CompanionConfig, CompanionData, int>,
        ICompanionsModel
    {

        #region ICompanionsModel implementation

        public int GetEquipmentSlotsCount => 5;

        // NOTE: why two different events?
        public event Action<int> OnEquip;
        public event Action<int> OnRemove;
        
        public Dictionary<int,string> GetEquipData()
        {
            return GameData.EquippedCompanionsData;
        }

        #endregion
        
        // private
        
        private CompanionsModel(IGameProgressModel progressModel,
            ISpritesModel spritesModel) : base(progressModel, spritesModel)
        {
        }

        #region BaseSummonModel overrides
        
        protected override string ConfigPath => "Companions";
        protected override Dictionary<string, ProgressData> GetProgressData()
        {
            var data = GameData.CompanionsData;
            if (data == null)
            {
                data = new Dictionary<string, ProgressData>();
                GameData.CompanionsData = data;
            }

            return data;
        }

        protected override Dictionary<int, string> GetEquippedData()
        {
            var data = GameData.EquippedCompanionsData;
            if (data == null)
            {
                data = new Dictionary<int, string>();
                GameData.EquippedCompanionsData = data;
            }

            return data;
        }

        protected override int GetEquipmentKey(CompanionData _)
        {
            return SetSelectedIndex;
        }
        
        public void Equip(string id, int index, bool shouldEquip = true)
        {
            SetSelectedIndex = index;
            base.Equip(id, shouldEquip);

            if (!shouldEquip) OnRemove?.Invoke(index);
            else OnEquip?.Invoke(index);
        }

        protected override bool ShouldUseActiveValueInParameterCalculations => false;

        #endregion
        
        private int SetSelectedIndex { get; set; }
    }
}