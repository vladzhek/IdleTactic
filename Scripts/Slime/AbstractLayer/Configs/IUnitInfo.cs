using Slime.AbstractLayer;

namespace Slime.AbstractLayer.Configs
{
    public interface IUnitInfo
    {
        string ID { get; }
        IUnitAvatar UnitAvatar { get; }
    }
}