using Slime.Configs.Abstract;
using Slime.Data.Products;
using UnityEngine;

namespace Slime.Configs.Products
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class ProductsConfig : ItemsConfig<ProductConfig, ProductData>
    {
        public const string PATH = ENTITY;
        private const string ENTITY = "Products";
    }
}