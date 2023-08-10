using Cysharp.Threading.Tasks;

namespace Slime.AbstractLayer
{
    public interface ISavable
    {
        public UniTask Save();
        public UniTask Load();
    }
}