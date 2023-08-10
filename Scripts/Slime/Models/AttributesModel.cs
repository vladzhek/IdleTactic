using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using Slime.Configs.Attributes;
using Slime.Data.Enums;
using Slime.Data.Products;
using Slime.Data.Progress.Abstract;
using Slime.Exceptions;
using Slime.Models.Abstract;
using AttributeData = Slime.Data.Attributes.AttributeData;

namespace Slime.Models
{
    public class AttributesModel : 
        BaseUpgradableModel<AttributesConfig, AttributeConfig, AttributeData>,
        IAuthorizedResourceUser,
        IAttributesModel
    {
        public string AuthorizationToken => Data.Constants.System.RESOURCE_TOKEN;
        
        #region IInitializable, IDisposable overrides
        
        public override void Initialize()
        {
            base.Initialize();

            _resourcesModel.OnChange += OnResourcesChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _resourcesModel.OnChange -= OnResourcesChanged;
        }

        #endregion
        
        #region BaseUpgradableModel overrides

        protected override string ConfigPath => "Attributes";
        protected override Dictionary<string, ProgressData> GetProgressData()
        {
            var data = GameData.AttributesData;
            if (data == null)
            {
                data = new Dictionary<string, ProgressData>();
                GameData.AttributesData = data;
            }

            return data;
        }

        protected override AttributeData PrepareData(AttributeData source)
        {
            var data = base.PrepareData(source);
            if (data.IsUpgradable) 
            {
                data.Sprite = _spritesModel.Get(data.SpriteId);
                data.IsEnoughResources = _resourcesModel.IsEnough(EResource.SoftCurrency, data.UpgradeCost);
            }
            return data;
        }
        
        public override void Upgrade(string id)
        {
            var data = Get(id);
            var resourceData = new ResourceData(EResource.SoftCurrency, data.UpgradeCost);
            // just to be sure
            data.IsEnoughResources = _resourcesModel.IsEnough(resourceData);
            if (data.IsEnoughResources)
            {
                if (!_resourcesModel.TrySpend(this, resourceData))
                {
                    throw new NotEnoughResourceException();
                }
            }
            
            base.Upgrade(id);
        }

        #endregion
        
        // private

        private readonly ISpritesModel _spritesModel;
        private readonly IResourcesModel _resourcesModel;
        
        private AttributesModel(IGameProgressModel progressModel, 
            ISpritesModel spritesModel,
            IResourcesModel resourcesModel) : base(progressModel)
        {
            _spritesModel = spritesModel;
            _resourcesModel = resourcesModel;
        }
        
        private void OnResourcesChanged(EResource resource, float _)
        {
            if (!resource.Equals(EResource.SoftCurrency))
            {
                return;
            }

            UpdateState();
        }

        public int IndexOf(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}