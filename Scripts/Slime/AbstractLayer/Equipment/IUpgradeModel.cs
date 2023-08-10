namespace Slime.AbstractLayer.Models.Equipment
{
    // TODO: refactor/remove
    public interface IUpgradeModel
    {
        public float GetCurrentValue(string id);
        public float GetNextValue(string id);
        public bool HasNextUpgrade(string id);
        public int GetNextUpgradePrice(string id);
        public bool IsUpgradeAvailable(string id);
        public bool TryUpgrade(string id);
    }
}