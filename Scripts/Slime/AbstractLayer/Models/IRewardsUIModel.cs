using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Slime.Data.Products;

namespace Slime.AbstractLayer.Models
{
    public interface IRewardsUIModel
    {
        public IEnumerable<ResourceData> Get();
        public UniTask Open(ResourceData data);
        public void Close();
    }
}