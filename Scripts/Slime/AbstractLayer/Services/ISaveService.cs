using Cysharp.Threading.Tasks;
using Slime.Data.Progress;

namespace Slime.AbstractLayer.Services
{
    public interface ISaveService
    {
        public UniTask Save(GameData gameData);
        public UniTask<GameData> Load();
    }
}