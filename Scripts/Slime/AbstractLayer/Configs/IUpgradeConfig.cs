namespace Slime.AbstractLayer.Configs
{
    public interface IUpgradeConfig
    {
        public int MaxLevel { get; }
        float GetValueForLevel(int level);
        float GetPriceForLevel(int level);
    }
}