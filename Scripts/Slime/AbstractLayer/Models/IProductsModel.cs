using Cysharp.Threading.Tasks;
using Slime.AbstractLayer.Equipment;
using Slime.Data.Abstract;

namespace Slime.AbstractLayer.Models
{
    public interface IProductsModel : IDisplayableModel<IProduct>
    {
        UniTask Purchase(string id);
    }
}