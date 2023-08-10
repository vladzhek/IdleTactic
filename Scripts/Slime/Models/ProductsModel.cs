using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Configs.Products;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.Data.Products;
using Slime.Exceptions;
using Slime.Models.Abstract;
using Logger = Utils.Logger;

namespace Slime.Models
{
    public class ProductsModel : 
        BaseDisplayableModel<ProductsConfig, ProductConfig, ProductData>,
        IProductsModel,
        IAuthorizedResourceUser
    {
        #region IAuthorizedResourceUser implementation

        public string AuthorizationToken => Data.Constants.System.RESOURCE_TOKEN;
        
        #endregion
        
        #region BaseDisplayableModel overrides
        
        protected override string ConfigPath => ProductsConfig.PATH;

        protected override ProductData PrepareData(ProductData source)
        {
            var data = base.PrepareData(source);
            if (data.Type != EProductType.Real)
            {
                return data;
            }
            
            var inAppData = _purchasingService.Get(data.ID);
            if (inAppData == null)
            {
                Logger.Warning($"product {data.ID} has no associated inApp data");
                return data;
            }

            data.SetInApp(inAppData);
            return data;
        }

        protected override void TriggerOnItemChange(ProductData data)
        {
            base.TriggerOnItemChange(data);
            
            OnItemChange?.Invoke(data);
        }
        
        #endregion

        #region IProductModel

        public new event Action<IProduct> OnItemChange;

        public new IEnumerable<IProduct> Get()
        {
            return base.Get();
        }

        public new IProduct Get(string id)
        {
            return base.Get(id);
        }

        public async UniTask Purchase(string id)
        {
            //Logger.Log($"id: {id}");

            var data = Get(id);
            switch (data.Type)
            {
                case EProductType.Real:
                    // process payment
                    if (!await _purchasingService.Purchase(data.ID)) throw new PurchaseNotSuccessfulException();
                    break;
                case EProductType.Virtual:
                default:
                    // check if enough resources
                    if (!_resourcesModel.IsEnough(data.Price)) throw new NotEnoughResourceException();
                    if (!_resourcesModel.TrySpend(this, data.Price)) throw new PurchaseNotSuccessfulException(); 
                    break;
            }
            
            // process resources
            if (!_resourcesModel.TryAdd(this, data.Resource)) throw new RewardNotSuccessfulException();
        }
        
        #endregion

        // private

        private readonly IResourcesModel _resourcesModel;
        private readonly IPurchasingService _purchasingService;
        
        private ProductsModel(IResourcesModel resourcesModel, IPurchasingService purchasingService)
        {
            _resourcesModel = resourcesModel;
            _purchasingService = purchasingService;
        }
    }
}