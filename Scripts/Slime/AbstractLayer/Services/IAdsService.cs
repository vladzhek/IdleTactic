using Cysharp.Threading.Tasks;

namespace Slime.AbstractLayer.Services
{
    public interface IAdsService
    {
        UniTask<bool> ShowAd(string placement);
    }
}