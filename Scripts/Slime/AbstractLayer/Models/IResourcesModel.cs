using System;
using Slime.Data.Enums;
using Slime.Data.Products;

namespace Slime.AbstractLayer.Models
{
    public interface IResourcesModel
    {
        public event Action<EResource, float> OnChange; 
        public float Get(EResource resource);
        public bool IsEnough(EResource resource, float quantity = 1);
        public bool IsEnough(ResourceData resource);
        public bool TryAdd(IAuthorizedResourceUser resourceUser, EResource resource, float quantity = 1);
        public bool TryAdd(IAuthorizedResourceUser resourceUser, ResourceData resource);
        public bool TrySpend(IAuthorizedResourceUser resourceUser, EResource resource, float quantity = 1);
        public bool TrySpend(IAuthorizedResourceUser resourceUser, ResourceData resource);
    }
}