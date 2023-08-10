using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.Triggers;

namespace Slime.GameStates
{
    [UsedImplicitly]
    public class QuitState : State<EGameTriggers>
    {
        private readonly IGameProgressModel _progressModel;
        
        public QuitState(
            IGameStateMachine gameStateMachine, 
            IGameProgressModel progressModel) : base(gameStateMachine)
        {
            _progressModel = progressModel;
        }

        public override void OnEnter()
        {
            _ = Save();
        }

        public override void OnExit()
        {
        }
        
        private async UniTask Save()
        {
            await UniTask.SwitchToMainThread();
            await _progressModel.Save();
        }
    }
}