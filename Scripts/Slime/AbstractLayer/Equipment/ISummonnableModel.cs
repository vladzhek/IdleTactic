using Slime.Data.Abstract;
using Slime.Data.Enums;

namespace Slime.AbstractLayer.Equipment
{
    public interface ISummonnableModel<out TData> :
        IDisplayableModel<TData>, 
        IUpgradableModel<TData>
        where TData : ISummonable
    {
        public TData GetRandomConfigItem(ERarity rarity);
    }
    
    public interface ISummonnableModel<out TData, in TEnum> : 
        ISummonnableModel<TData>,
        IEquipableModel<TData, TEnum>
        where TData : ISummonable
    {
    }
}