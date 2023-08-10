using Slime.AbstractLayer;
using Slime.AbstractLayer.Configs;
using Slime.AbstractLayer.Services;
using Slime.Data.Progress;
using Slime.Levels;
using Zenject;

namespace Slime.Factories
{
    public class StageFactory : IStageFactory
    {
        private readonly UnitFactory _unitFactory;

        private readonly IPlayer _player;

        [Inject]
        private StageFactory(UnitFactory unitFactory, IPlayer player)
        {
            _player = player;
            _unitFactory = unitFactory;
        }

        public IStage Load(IStageConfig config, StageData data)
        {
            return new Stage(config, data, _unitFactory, _player.Unit);
        }
    }
}