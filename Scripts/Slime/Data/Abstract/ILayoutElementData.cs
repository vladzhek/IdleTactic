using System;

namespace Slime.Data.Abstract
{
    public interface ILayoutElementData : IDisplayable, IEquipable, IUpgradable, IEquatable<ILayoutElementData>
    {
        new string ID { get; }
        new bool IsUnlocked { get; }
        bool NeedsAttention { get; }
    }
}