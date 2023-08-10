using Slime.Configs.Abstract;
using Slime.Data.Inventory;
using UnityEngine;

namespace Slime.Configs.Inventory
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class InventoryItemConfig : ItemConfig<InventoryData>
    {
        private const string ENTITY = "InventoryItem";
    }
}