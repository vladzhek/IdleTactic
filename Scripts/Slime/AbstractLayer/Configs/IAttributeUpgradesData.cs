namespace Slime.AbstractLayer.Configs
{
    public interface IAttributeUpgradesData
    {
        string AttributeID { get; }
        IUpgradeConfig Config { get; }
    }
}