using System;
using System.Collections.Generic;
using Slime.Data.Abstract;

namespace Slime.AbstractLayer.Equipment
{
    public interface IEquipableModel<out TData, in TEnum> where TData : IEquipable
    {
        public event Action<string, bool> OnEquip;
        public IEnumerable<TData> GetEquipped();
        public TData GetEquipped(TEnum type);
        public void Equip(string id, bool shouldEquip);
    }
}