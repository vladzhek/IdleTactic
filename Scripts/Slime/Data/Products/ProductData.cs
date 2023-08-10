using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using Slime.Data.Abstract;
using Slime.Data.Enums;
using Slime.Data.IDs;
using UnityEngine;
using Utils.Extensions;

namespace Slime.Data.Products
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Serializable]
    public class ProductData : GenericData, IProduct
    {
        #region IProduct
        
        public override string ID => _purchaseID;
        public override string SpriteID => $"product{base.SpriteID.Capitalize()}";
        public EProductType Type => _type;
        public string Title => _title;
        public Sprite Sprite { get; set; }
        public ResourceData Resource => _resource;
        public ResourceData Price => _price;
        public string LocalizedPrice { get; private set; }
        public bool IsSale => _isSale;
        public bool IsBonus => _isBonus;
        
        #endregion

        public void SetInApp(InAppData data)
        {
            if (_type != EProductType.Real)
            {
                throw new Exception($"type {_type} can't be in app purchase");
            }
            
            _price = new ResourceData(EResource.RealCurrency, data.Price);
            LocalizedPrice = data.LocalizedPrice;
        }
        
        // private
        
        [Header("Product")]
        [SerializeField, ValueDropdown(nameof(IDs))] private string _purchaseID;
        [SerializeField] private EProductType  _type;
        [SerializeField] private string  _title;
        [Space]
        [SerializeField] private ResourceData  _resource;
        [SerializeField, ShowIf("_type", EProductType.Virtual)] private ResourceData  _price;
        [Space] 
        [SerializeField] private bool _isSale;
        [SerializeField] private bool _isBonus;
        
        private IEnumerable<string> IDs => ProductIDs.Values;
    }
}