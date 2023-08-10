using System;
using Slime.Data.Enums;

namespace Slime.AbstractLayer.Models
{
    public interface IInventoryUIModel
    {
        public event Action OnChange;
        public EInventoryType Type { get; }
        public void SetInventoryType(EInventoryType type);
    }
}