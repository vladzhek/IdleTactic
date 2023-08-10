using System;
using Slime.AbstractLayer.Equipment;
using Slime.Data.Skills;

namespace Slime.AbstractLayer.Models
{
    public interface ISkillsModel : ISummonnableModel<SkillData, int>
    {
        event Action<int, string> OnEquip;
        event Action<int, string> OnRemove;
        
        public void Equip(string id, int index, bool shouldEquip = true);
        public float GetTotalPassiveEffect();
        public bool IsLoaded { get; }
    }
}