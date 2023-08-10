using System.Collections.Generic;
using Slime.AbstractLayer.Equipment;
using Slime.Data.Enums;
using Slime.Data.Inventory;

namespace Slime.AbstractLayer.Models
{
    public interface IInventoryModel : ISummonnableModel<InventoryData, EInventoryType>
    {
        public IEnumerable<InventoryData> Get(EInventoryType type);
    }
}