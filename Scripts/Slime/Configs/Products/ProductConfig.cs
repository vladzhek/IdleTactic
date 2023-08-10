using Slime.Configs.Abstract;
using Slime.Data.Inventory;
using Slime.Data.Products;
using UnityEngine;

namespace Slime.Configs.Products
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class ProductConfig : ItemConfig<ProductData>
    {
        private const string ENTITY = "Product";
    }
}