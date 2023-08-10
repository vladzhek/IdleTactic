using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Slime.Data.Products;
using Slime.Models.Abstract;
using Utils;

namespace Slime.Models
{
    [UsedImplicitly]
    public class ResourcesModel : BaseProgressModel, IResourcesModel
    {
        #region IResources implementation

        public event Action<EResource, float> OnChange;
        
        public float Get(EResource resource)
        {
            return GetData(resource).Quantity;
        }

        public bool IsEnough(EResource resource, float quantity = 1)
        {
            return GetData(resource).Quantity >= quantity;
        }

        public bool IsEnough(ResourceData resource)
        {
            return IsEnough(resource.Resource, resource.Quantity);
        }

        public bool TryAdd(IAuthorizedResourceUser resourceUser, EResource resource, float quantity = 1)
        {
            if (!IsAuthorized(resourceUser))
            {
                return false;
            }

            var resourceData = GetData(resource);
            resourceData += quantity;
            //Logger.Log($"resource: {resource}; quantity: {resourceData.Quantity}");
            OnChange?.Invoke(resource, resourceData.Quantity);
            return true;
        }

        public bool TryAdd(IAuthorizedResourceUser resourceUser, ResourceData resource)
        {
            return TryAdd(resourceUser, resource.Resource, resource.Quantity);
        }

        public bool TrySpend(IAuthorizedResourceUser resourceUser, EResource resource, float quantity = 1)
        {
            if (!IsAuthorized(resourceUser))
            {
                return false;
            }

            var resourceData = GetData(resource);
            if (resourceData.Quantity < quantity)
            {
                Logger.Warning($"not enough ${resource}");
                return false;
            }
            
            resourceData -= quantity;
            OnChange?.Invoke(resource, resourceData.Quantity);
            return true;
        }

        public bool TrySpend(IAuthorizedResourceUser resourceUser, ResourceData resource)
        {
            return TrySpend(resourceUser, resource.Resource, resource.Quantity);
        }

        #endregion
        
        // private
        
        private const string AUTHORIZATION_TOKEN = Data.Constants.System.RESOURCE_TOKEN;
        
        private ResourcesModel(IGameProgressModel progressModel) : base(progressModel)
        {
        }
        
        private ResourceData GetData(EResource resource)
        {
            var resourcesData = GameData.ResourcesData;
            if (resourcesData == null)
            {
                resourcesData = new Dictionary<EResource, ResourceData>();
                GameData.ResourcesData = resourcesData;
            }

            var resourceData = resourcesData.GetValueOrDefault(resource);
            if (resourceData == null)
            {
                resourceData = new ResourceData(resource, resource.GetInitialQuantity());
                resourcesData.Add(resource, resourceData);
            }
            
            return resourceData;
        }
        
        private static bool IsAuthorized(IAuthorizedResourceUser resourceUser)
        {
            if (!resourceUser.AuthorizationToken.Equals(AUTHORIZATION_TOKEN))
            {
                Logger.Warning($"class {resourceUser.GetType()} is not authorised resource user");
                return false;
            }

            return true;
        }
    }
}