using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Configs.Inventory;
using Slime.Data.Enums;
using Slime.Data.Inventory;
using Slime.Data.Progress.Abstract;
using Slime.Models.Abstract;

namespace Slime.Models
{
    [UsedImplicitly]
    public class InventoryModel : 
        BaseSummonnableModel<InventoryConfig, InventoryItemConfig, InventoryData, EInventoryType>,
        IInventoryModel
    {
        #region IInventoryModel implementation
        
        public IEnumerable<InventoryData> Get(EInventoryType type)
        {
            return from item in Get() where item.Type == type select item;
        }
        
        #endregion
        
        private InventoryModel(IGameProgressModel progressModel,
            ISpritesModel spritesModel) : base(progressModel, spritesModel)
        {
        }

        #region BaseSummonModel overrides
        
        protected override string ConfigPath => "Inventory";
        protected override Dictionary<string, ProgressData> GetProgressData()
        {
            var data = GameData.InventoryData;
            if (data == null)
            {
                data = new Dictionary<string, ProgressData>();
                GameData.InventoryData = data;
            }

            return data;
        }

        protected override Dictionary<EInventoryType, string> GetEquippedData()
        {
            var data = GameData.EquippedInventoryData;
            if (data == null)
            {
                data = new Dictionary<EInventoryType, string>();
                GameData.EquippedInventoryData = data;
            }

            return data;
        }

        protected override EInventoryType GetEquipmentKey(InventoryData data)
        {
            return data.Type;
        }

        protected override EAttribute DataToParameter(InventoryData data)
        {
            return data.Type switch
            {
                EInventoryType.Armor => EAttribute.Health,
                _ => EAttribute.Damage
            };
        }
        
        #endregion
    }
}