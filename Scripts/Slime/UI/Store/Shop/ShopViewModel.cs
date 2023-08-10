using System;
using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer.Models;
using Slime.Data.Abstract;
using Slime.Data.Dialog;
using UI.Base.MVVM;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.UI.Store.Shop
{
    public class ShopViewModel : ViewModel
    {
        public event Action<int, ShopLayoutData> OnItemChange;
        
        public IEnumerable<ShopLayoutData> Get() => from item in _productsModel.Get()
            select ToViewData(item);
        
        public async void PurchaseItem(string id)
        {
            try
            {
                await _productsModel.Purchase(id);
            }
            catch (Exception e)
            {
                Logger.Warning($"{e}");
                
                _ = _dialogUIModel.Open(new DialogData("error", e.Message));
                
                return;
            }
            
            _ = _dialogUIModel.Open(new DialogData("success", "congratulations on your purchase!"));
        }
        
        #region ViewModel overrides
        
        public override void OnSubscribe()
        {
            base.OnSubscribe();

            _productsModel.OnItemChange += OnItemChanged;
        }
        
        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _productsModel.OnItemChange += OnItemChanged;
        }
        
        #endregion

        // private

        private readonly IProductsModel _productsModel;
        private readonly ISpritesModel _spritesModel;
        private readonly IDialogUIModel _dialogUIModel;
        
        private ShopViewModel(IProductsModel productsModel, 
            ISpritesModel spritesModel,
            IDialogUIModel dialogUIModel
            )
        {
            _productsModel = productsModel;
            _spritesModel = spritesModel;
            _dialogUIModel = dialogUIModel;
        }
        
        private void OnItemChanged(IProduct item)
        {
            var index = _productsModel.IndexOf(item.ID);
            OnItemChange?.Invoke(index, ToViewData(item));
        }

        private ShopLayoutData ToViewData(IProduct product)
        {
            Sprite sprite = null;
            var spriteId = product.SpriteID;
            if (product.Sprite == null && !string.IsNullOrEmpty(spriteId))
            {
                sprite = _spritesModel.Get(spriteId);
            }
            return new ShopLayoutData(product, sprite);
        }
    }
}