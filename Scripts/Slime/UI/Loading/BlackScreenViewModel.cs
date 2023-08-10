using Reactive;
using Slime.AbstractLayer.Models;
using UI.Base;
using UI.Base.MVVM;

namespace Slime.UI
{
    public class BlackScreenViewModel : ViewModel
    {
        private readonly IStageModel _stageModel;
        private readonly UIManager _uiManager;

        public ReactiveProperty<bool> IsActive { get; }

        public BlackScreenViewModel(IStageModel stageModel, UIManager uiManager)
        {
            _stageModel = stageModel;
            _uiManager = uiManager;
            IsActive = new ReactiveProperty<bool>(_stageModel.IsScreenHidden);
        }

        public override void OnSubscribe()
        {
            _stageModel.OnScreenHiddenChange += OnScreenVisibilityChanged;
        }

        public override void OnUnsubscribe()
        {
            _stageModel.OnScreenHiddenChange -= OnScreenVisibilityChanged;
        }

        public override void OnDispose()
        {
        }

        private void OnScreenVisibilityChanged(bool isScreenHidden)
        {
            IsActive.Value = isScreenHidden;
        }
    }
}