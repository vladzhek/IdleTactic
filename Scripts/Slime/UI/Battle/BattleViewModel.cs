using System;
using System.Collections.Generic;
using Reactive;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Data.Constants;
using Slime.Data.Enums;
using Slime.Data.Skills;
using Slime.UI.Boosters;
using Slime.UI.Character.Skills;
using Slime.UI.Footer.Character;
using Slime.UI.Footer.Dungeons;
using Slime.UI.Popups;
using UI.Base.MVVM;
using Utils.Time;

namespace Slime.UI.Battle
{
    public class BattleViewModel : ViewModel
    {
        public const int DEACTIVATE_SLIDER = -1;

        public event Action UpdateSkillsPanel;
        public event Action<bool> OnTutorialActive;
        public event Action<float> TimeLeft;

        private readonly ISkillsManager _skillsManager;
        private readonly ISkillsModel _skillsModel;
        private readonly IStageModel _stageModel;
        private readonly IStageController _stageController;
        private readonly UIManager _uiManager;
        private readonly IOfflineIncomeModel _offlineIncomeModel;
        private readonly ITutorialModel _tutorialModel;
        private readonly TimerService _timerService;
        private readonly ICharacterUIModel _characterUIModel;
        private readonly IFooterUIModel _footerUIModel;

        public ReactiveProperty<string> LevelInfo { get; private set; }
        public ReactiveProperty<float> LevelForSlider { get; private set; }
        public ReactiveProperty<bool> IsAutoSkillActive { get; private set; }
        public ReactiveProperty<bool> IsRetryButtonActive { get; private set; }

        public Dictionary<int, SkillPanelViewData> SkillsPanelViewData = new();

        private BattleViewModel(ISkillsModel skillsModel, IStageController stageController,
            IStageModel stageModel, UIManager uiManager, IOfflineIncomeModel offlineIncomeModel,
            ISkillsManager skillsManager, ITutorialModel tutorialModel, TimerService timerService,
            ICharacterUIModel characterUIModel, IFooterUIModel footerUIModel)

        {
            _stageController = stageController;
            _stageModel = stageModel;
            _skillsModel = skillsModel;
            _uiManager = uiManager;
            _offlineIncomeModel = offlineIncomeModel;
            _skillsManager = skillsManager;
            _tutorialModel = tutorialModel;
            _timerService = timerService;
            _characterUIModel = characterUIModel;
            _footerUIModel = footerUIModel;
            
            LevelInfo = new ReactiveProperty<string>("LVL ?-?");
            LevelForSlider = new ReactiveProperty<float>(0);
            IsRetryButtonActive = new ReactiveProperty<bool>(_stageModel.IsBattleCycled);
            IsAutoSkillActive = new ReactiveProperty<bool>(true);
        }

        public event Action<int> OnOfflineIncome
        {
            add => _offlineIncomeModel.OnOfflineIncome += value;
            remove => _offlineIncomeModel.OnOfflineIncome -= value;
        }

        public event Action<IStageController> OnStageStarted
        {
            add => _stageController.StageStarted += value;
            remove => _stageController.StageStarted -= value;
        }

        public event Action<IStageController> OnDungeonEnded;

        public override void OnInitialize()
        {
            base.OnInitialize();

            UpdateLevelInfo();
            InitializeSkillsPanelData();
            SetTutorialStage(_tutorialModel.Stage);
        }

        public override void OnSubscribe()
        {
            _stageModel.OnCycleChange += CycleChanged;
            _stageController.StageChanged += OnStageProceed;
            _skillsModel.OnChange += UpdateState;
            _tutorialModel.OnChange += SetTutorialStage;
            _stageController.StageFailed += StageEnd;
            _stageController.StageCompleted += StageEnd;
        }

        public override void OnUnsubscribe()
        {
            _stageModel.OnCycleChange -= CycleChanged;
            _stageController.StageChanged -= OnStageProceed;
            _skillsModel.OnChange -= UpdateState;
            _tutorialModel.OnChange -= SetTutorialStage;
            _stageController.StageFailed -= StageEnd;
            _stageController.StageCompleted -= StageEnd;
        }

        private void UpdateState()
        {
            InitializeSkillsPanelData();

            UpdateSkillsPanel?.Invoke();
        }

        private void InitializeSkillsPanelData()
        {
            for (var index = 0; index < _skillsManager.MaxSkillsCount; index++)
            {
                var viewData = new SkillPanelViewData
                {
                    // NOTE: wtf?
                    ID = index.ToString(),
                };

                var data = _skillsModel.GetEquipped(index);

                if (data != null)
                {
                    viewData = new SkillPanelViewData
                    {
                        ID = data.ID,
                        Sprite = data.Sprite,
                        IsOpened = true,
                        IsEquipped = true, // NOTE: why skill panel need isEquipped if it contains only equipped? 
                    };
                }
                else
                {
                    viewData = new SkillPanelViewData
                    {
                        IsOpened = true
                    };
                }

                SkillsPanelViewData[index] = viewData;
            }
        }

        public void ToggleAutoSkill()
        {
            _skillsManager.SetAutoSkill(!_skillsManager.IsAutoSkillActive);
            IsAutoSkillActive.Value = _skillsManager.IsAutoSkillActive;
        }

        public void TryAgain()
        {
            if (_stageModel.Type == EStageType.Default)
            {
                _stageModel.SetCycled(false);
            }
        }

        public void ShowBoosterView()
        {
            _uiManager.Open<BoostersViewPopup>();
        }

        private void OnStageProceed(IStageController stageController)
        {
            UpdateLevelInfo();

            if (stageController.Type != EStageType.Default)
            {
                if (_timerService.Timers.ContainsKey(stageController.Type.ToString()))
                {
                    _timerService.Timers[stageController.Type.ToString()].OnTick += OnTimerLeft;
                }
            }
        }

        private void UpdateLevelInfo()
        {
            var stage = _stageController.CurrentStage;
            if (stage == null)
            {
                LevelInfo.Value = $"LVL ?-?";
                return;
            }

            var stageType = _stageModel.Type;

            LevelInfo.Value = stageType != EStageType.Default
                ? stageType.ToTitle()
                : $"LVL {stage.StageData.Stage + 1}-{stage.StageData.Wave + 1}";

            var sliderProgress = (float)(stage.StageData.Wave!.Value + 1) /
                                 _stageController.CurrentStage.StageConfig.Waves.Length;

            LevelForSlider.Value = stageType != EStageType.Default || _stageModel.IsBattleCycled
                ? DEACTIVATE_SLIDER
                : sliderProgress;
        }

        public void UseSkill(string ID)
        {
            _skillsManager.ActivateSkill(ID);

            if (ID == null)
            {
                _footerUIModel.SelectTab(Values.FOOTER_TAB_CHARACTER);
                _characterUIModel.SelectTab(1);
            }
        }

        public void ShowOfflineIncomeView()
        {
            _uiManager.Open<OfflineIncomePopupView>();
        }

        public float GetSecondsToCooldownSkill(string viewDataID)
        {
            if (viewDataID == null)
                return 0;

            if (_skillsManager.Skills.TryGetValue(viewDataID, out var skill))
            {
                return skill.Cooldown / skill.SecondsToCooldown;
            }

            return 0;
        }

        private void CycleChanged(bool IsActive)
        {
            IsRetryButtonActive.Value = IsActive;
        }

        private void SetTutorialStage(ETutorialStage stage)
        {
            OnTutorialActive?.Invoke(ETutorialStage.StageReplay == stage);
        }

        private void OnTimerLeft(int timeLeft)
        {
            TimeLeft?.Invoke(timeLeft);
        }

        public float GetTimerDuration(string id)
        {
            return _timerService.Timers[id].Duration;
        }

        private void StageEnd(IStageController stageController)
        {
            if (stageController.Type != EStageType.Default)
            {
                _uiManager.Open<DungeonsEntryView>();
                OnDungeonEnded?.Invoke(stageController);
            }
        }
    }
}