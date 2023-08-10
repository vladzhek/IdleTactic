using System;

namespace Slime.AbstractLayer.Equipment
{
    public interface IUpgradableModel<out TData>
    {
        event Action<TData> OnUpgrade;
        public void Add(string id, int quantity);
        public void Upgrade(string id);
        public void UpgradeAll();
    }
}