namespace Slime.Data.Abstract
{
    public interface IEquipable
    {
        public bool IsEquipped { get; set; }
        public bool IsUnlocked { get; }
    }
}