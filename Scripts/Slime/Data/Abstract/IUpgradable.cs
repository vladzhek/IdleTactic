using Slime.Data.Progress.Abstract;

namespace Slime.Data.Abstract
{
    public interface IUpgradable
    {
        public string ID { get; }
        public bool IsUnlocked { get; }
        public int Level { get; }
        public float Quantity { get; }
        public int UpgradeQuantity { get; }
        public float Progress { get; }
        public bool CanUpgrade { get; }
        
        public void SetProgress(ProgressData data);
        
        public void Upgrade();
        public void UpgradeOne();
    }
}