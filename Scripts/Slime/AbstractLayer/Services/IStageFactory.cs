using Slime.AbstractLayer.Configs;
using Slime.Data.Progress;

namespace Slime.AbstractLayer.Services
{
    public interface IStageFactory
    {
        public IStage Load(IStageConfig config, StageData data);
    }
}