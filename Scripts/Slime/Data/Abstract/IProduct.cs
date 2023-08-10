using Slime.Data.Enums;
using Slime.Data.Products;
using UnityEngine;

namespace Slime.Data.Abstract
{
    public interface IProduct : IData
    {
        EProductType Type { get; }
        string Title { get; }
        Sprite Sprite { get; }
        
        ResourceData Resource { get; }
        ResourceData Price { get; }
        string LocalizedPrice { get; }
        
        bool IsSale { get; }
        bool IsBonus { get; }
    }
}