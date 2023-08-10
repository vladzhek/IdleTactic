using System;
using Cysharp.Threading.Tasks;
using Slime.Data.Products;
using Zenject;

namespace Slime.AbstractLayer.Services
{
    public interface IPurchasingService : IInitializable, IDisposable
    {
        // TODO: add checkups and init attempts to ShopModel
        public bool IsInit { get; }
        public UniTask<bool> TryInit();
        public InAppData Get(string id);
        public UniTask<bool> Purchase(string id);
    }
}