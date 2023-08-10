using System;
using Slime.Data.Abstract;

namespace Slime.AbstractLayer.Models
{
    public interface IEquipmentItemUIModel
    {
        event Action OnEquipRequest;
        event Action OnUpgradeRequest;
        ISummonable Item { get; }

        public void Open(ISummonable item);
        public void Close();
        public void RequestEquip();
        public void RequestUpgrade();
    }
}