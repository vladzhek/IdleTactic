using System.Linq;
using Cysharp.Threading.Tasks;
using MadPixel.InApps;
using Slime.AbstractLayer.Services;
using Slime.Data.IDs;
using Slime.Data.Products;
using UnityEngine;
using UnityEngine.Purchasing;
using Application = UnityEngine.Device.Application;
using Logger = Utils.Logger;

namespace Slime.Services.Purchasing
{
    public class MadPixelsPurchasingService : IPurchasingService
    {
        #region IInitializable, IDisposable implementation
        
        public async void Initialize()
        {
            await UniTask.WaitUntil(() => Exists);

            Instance.OnPurchaseResult += OnPurchaseResult;
            Instance.OnInitialize += OnInitialized;
            
            _ = TryInit();
        }

        public void Dispose()
        {
            if (Exists)
            {
                Instance.OnPurchaseResult -= OnPurchaseResult;
                Instance.OnInitialize -= OnInitialized;
            }
        }

        #endregion
        
        #region IPurchasingService implementation

        public bool IsInit => _isInit;

        public UniTask<bool> TryInit()
        {
            if (_initCompletionSource != null)
            {
                Logger.Warning($"previous init attempt is ongoing");
                return UniTask.FromResult(false);
            }
            
            _initCompletionSource = new UniTaskCompletionSource<bool>();
            Instance.Init(ProductIDs.NO_ADS_SKU, ProductIDs.InApps.ToList());

            return _initCompletionSource.Task;
        }
        
        public UniTask<bool> Purchase(string id)
        {
            //Logger.Log($"id: {id}");

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Logger.Warning($"internet is not reachable");
                return UniTask.FromResult(false);
            }

            if (!IsReady)
            {
                Logger.Warning($"mad pixel purchasing service does not exist or is not init");
                return UniTask.FromResult(false);
            }
            
            if (_purchaseCompletionSource != null)
            {
                Logger.Warning($"previous product is being purchased");
                return UniTask.FromResult(false);
            }
            
            // DEBUG: purchase without store data
            return UniTask.FromResult(true);
            
            _purchaseCompletionSource = new UniTaskCompletionSource<bool>();
            _isPurchased = Instance.BuyProductInner(id);
            if (!_isPurchased)
            {
                _purchaseCompletionSource = null;
            }
            
            //Logger.Log($"isSuccess: {_taskResult}; completion: {_taskCompletionSource}");
            return _purchaseCompletionSource?.Task ?? UniTask.FromResult(_isPurchased);
        }

        public InAppData Get(string id)
        {
            var data = Instance.GetProduct(id)?.metadata;
            if (data == null)
            {
                Logger.Warning($"product \"{id}\" doesn't exist");
                return null;
            }
            
            return new InAppData(
                (float)data.localizedPrice, 
                data.localizedPriceString);
        }

        #endregion

        // private
        
        private static MobileInAppPurchaser Instance => MobileInAppPurchaser.Instance;

        private static bool Exists => MobileInAppPurchaser.Exist;
        private bool IsReady => Exists && _isInit;
        
        private UniTaskCompletionSource<bool> _initCompletionSource;
        private bool _isInit;

        private UniTaskCompletionSource<bool> _purchaseCompletionSource;
        private bool _isPurchased;
        
        private void OnPurchaseResult(Product product)
        {
            //throw new System.NotImplementedException();
            
            _isPurchased = !string.IsNullOrEmpty(product?.definition?.id);
            Logger.Log($"isPurchased: {_isPurchased}");
            _purchaseCompletionSource?.TrySetResult(_isPurchased);
            _purchaseCompletionSource = null;
        }
        
        private void OnInitialized(bool isInit)
        {
            _isInit = isInit;
            Logger.Log($"isInit: {isInit}");
            _initCompletionSource?.TrySetResult(_isInit);
            _initCompletionSource = null;
        }
    }
}