namespace Slime.Data.Abstract
{
    public interface ISummonable : IClonable, IDisplayable, IUpgradable, IEquipable 
    {
        public new string ID { get; }
        public new bool IsUnlocked { get; }
    }
}