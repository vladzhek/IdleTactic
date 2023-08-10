using Slime.Configs.Abstract;
using Slime.Data.Inventory;
using UnityEngine;

namespace Slime.Configs.Inventory
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class InventoryConfig : ItemsConfig<InventoryItemConfig, InventoryData>
    {
        private const string ENTITY = "Inventory";
    }
}