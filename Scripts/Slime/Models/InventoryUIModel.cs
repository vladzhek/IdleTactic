using System;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;

namespace Slime.Models
{
    public class InventoryUIModel : IInventoryUIModel
    {
        public event Action OnChange;
        
        public EInventoryType Type { get; private set; }

        public void SetInventoryType(EInventoryType type)
        {
            Type = type;
            OnChange?.Invoke();
        }
    }
}