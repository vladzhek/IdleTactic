using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Equipment;
using Slime.Data.Inventory;

namespace Slime.AbstractLayer.Models
{
    public interface ICompanionsModel : ISummonnableModel<CompanionData, int>
    {
        // TODO: remove new
        public new event Action<int> OnEquip;
        // TODO: merge with OnEquip
        public event Action<int> OnRemove;

        public int GetEquipmentSlotsCount { get; }

        // TODO: model should not be responsible about selection
        void Equip(string id, int index, bool shouldEquip = true);
        // TODO: model should not share this data
        Dictionary<int,string> GetEquipData();
    }
}