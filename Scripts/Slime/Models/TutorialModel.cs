using System;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Slime.GameStates;
using Slime.Models.Abstract;
using Slime.UI;
using Slime.UI.Tutorial;

namespace Slime.Models
{
    public class TutorialModel : BaseProgressModel, ITutorialModel
    {
        #region ITutorialModel implementation
        
        public event Action<string> OnDisplayMessage;
        public event Action<ETutorialStage> OnChange;
        
        public ETutorialStage Stage { get; private set; }

        public void SetTutorialType(ETutorialStage stage)
        {
            _settingsModel.Set(TUTORIAL_STAGE_KEY, stage);
            Stage = stage;
            OnChange?.Invoke(stage);
        }

        public void DisplayMessage(string message)
        {
            _uiManager.Open<TutorialView>();
            
            OnDisplayMessage?.Invoke(message);
        }
        
        #endregion
        
        // private
        
        private const string TUTORIAL_STAGE_KEY = "currentTutorialStage";

        private readonly IObjectFactory _objectFactory;
        private readonly ISettingsModel _settingsModel;
        private readonly UIManager _uiManager;

        private TutorialModel(IGameProgressModel progressModel, 
            ISettingsModel settingsModel, 
            IObjectFactory objectFactory,
            UIManager uiManager)
            : base(progressModel)
        {
            _settingsModel = settingsModel;
            _objectFactory = objectFactory;
            _uiManager = uiManager;
        }
        
        protected override void OnProgressLoaded()
        {
            base.OnProgressLoaded();

            Stage = _settingsModel.GetEnum(TUTORIAL_STAGE_KEY, ETutorialStage.Welcome);
            _ = new TutorialStateMachine(_objectFactory, this);
        }
    }
}