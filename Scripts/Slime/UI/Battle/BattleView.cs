using Slime.AbstractLayer.Services;
using Slime.Data.Enums;
using Slime.Data.Skills;
using Slime.UI.Battle;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Slime.UI
{
    public class BattleView : View<BattleViewModel>
    {
        [SerializeField] private Button _tryAgainButton;
        [SerializeField] private Button _autoSkillButton;
        [SerializeField] private Button _boosterButton;
        [SerializeField] private Button _offlineIncomeButton;

        [SerializeField] private TextMeshProUGUI _levelField;
        [SerializeField] private TextMeshProUGUI _timerText;

        [SerializeField] private GameObject _autoSkillActivityObject;
        [SerializeField] private RectTransform _tutorialFinger;

        [SerializeField] private SkillsPanel _skillsPanel;
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _watchFill;

        private float _timeDuration;

        public override UILayer Layer => UILayer.Background;

        protected override void OnEnable()
        {
            _autoSkillActivityObject.SetActive(ViewModel.IsAutoSkillActive.Value);
            _tryAgainButton.gameObject.SetActive(ViewModel.IsRetryButtonActive.Value);
            _skillsPanel.SetData(ViewModel.SkillsPanelViewData.Values);
            _levelField.text = ViewModel.LevelInfo.Value;

            _tryAgainButton.onClick.AddListener(OnTryAgainClicked);
            _autoSkillButton.onClick.AddListener(OnAutoSkillClicked);
            _boosterButton.onClick.AddListener(OpenBoosterView);
            _offlineIncomeButton.onClick.AddListener(OpenOfflineIncomeView);

            OnRetryButtonActivityChanged(ViewModel.IsRetryButtonActive.Value);
            SwitchTimer(false);

            ViewModel.IsAutoSkillActive.Changed += OnAutoSkillChanged;
            ViewModel.LevelInfo.Changed += OnLevelInfoChanged;
            ViewModel.LevelForSlider.Changed += OnSliderValue;
            ViewModel.IsRetryButtonActive.Changed += OnRetryButtonActivityChanged;
            ViewModel.OnOfflineIncome += OnOfflineIncome;
            ViewModel.UpdateSkillsPanel += OnUpdateSKillsPanel;
            ViewModel.OnTutorialActive += TutorialActive;
            ViewModel.OnStageStarted += StageStarted;
            ViewModel.TimeLeft += OnTimer;
            ViewModel.OnDungeonEnded += DungeonEnded;

            _skillsPanel.OnSelect += OnSkillSelected;

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _tryAgainButton.onClick.RemoveListener(OnTryAgainClicked);
            _autoSkillButton.onClick.RemoveListener(OnAutoSkillClicked);
            _boosterButton.onClick.RemoveListener(OpenBoosterView);
            _offlineIncomeButton.onClick.RemoveListener(OpenOfflineIncomeView);

            ViewModel.IsAutoSkillActive.Changed -= OnAutoSkillChanged;
            ViewModel.LevelInfo.Changed -= OnLevelInfoChanged;
            ViewModel.LevelForSlider.Changed -= OnSliderValue;
            ViewModel.IsRetryButtonActive.Changed -= OnRetryButtonActivityChanged;
            ViewModel.OnOfflineIncome -= OnOfflineIncome;
            ViewModel.UpdateSkillsPanel -= OnUpdateSKillsPanel;
            ViewModel.OnTutorialActive -= TutorialActive;
            ViewModel.OnStageStarted -= StageStarted;
            ViewModel.TimeLeft -= OnTimer;
            ViewModel.OnDungeonEnded -= DungeonEnded;

            _skillsPanel.OnSelect -= OnSkillSelected;
        }

        private void OnUpdateSKillsPanel()
        {
            _skillsPanel.SetData(ViewModel.SkillsPanelViewData.Values);
        }

        private void OnSliderValue(float wave)
        {
            if (SwitchSlider(wave))
            {
                return;
            }

            _slider.value = wave;
        }

        private bool SwitchSlider(float wave)
        {
            if (wave <= -1)
            {
                _slider.gameObject.SetActive(false);
                return true;
            }

            if (!_slider.gameObject.activeSelf)
            {
                _slider.gameObject.SetActive(true);
            }

            return false;
        }

        private void OnRetryButtonActivityChanged(bool isActive)
        {
            _tryAgainButton.gameObject.SetActive(isActive);
        }

        private void OnSkillSelected(SkillPanelViewData data)
        {
            ViewModel.UseSkill(data.ID);
        }

        private void Update()
        {
            for (var i = 0; i < ViewModel.SkillsPanelViewData.Count; i++)
            {
                var viewData = ViewModel.SkillsPanelViewData[i];
                viewData.Cooldown = ViewModel.GetSecondsToCooldownSkill(viewData.ID);
                _skillsPanel.SetData(i, viewData);
            }
        }

        private void OnLevelInfoChanged(string levelInfo)
        {
            _levelField.text = levelInfo;
        }

        private void OnAutoSkillChanged(bool isAutoSkill)
        {
            _autoSkillActivityObject.SetActive(isAutoSkill);
        }

        private void OnTryAgainClicked()
        {
            ViewModel.TryAgain();
        }

        private void OnAutoSkillClicked()
        {
            ViewModel.ToggleAutoSkill();
        }

        private void OpenBoosterView()
        {
            ViewModel.ShowBoosterView();
        }

        private void OpenOfflineIncomeView()
        {
            ViewModel.ShowOfflineIncomeView();
            _offlineIncomeButton.interactable = false;
        }

        private void OnOfflineIncome(int value)
        {
            _offlineIncomeButton.interactable = value > 0;
        }

        private void TutorialActive(bool IsActive)
        {
            _tutorialFinger.gameObject.SetActive(IsActive);
        }

        private void OnTimer(float time)
        {
            _timerText.text = FormatTime.MinutesStringFormat((int)time);
            _watchFill.fillAmount = time / _timeDuration;
        }

        private void StageStarted(IStageController stageController)
        {
            if (stageController.Type != EStageType.Default)
            {
                _timeDuration = ViewModel.GetTimerDuration(stageController.Type.ToString());
               SwitchTimer(true);
               OnRetryButtonActivityChanged(false);
            }
        }
        
        private void SwitchTimer(bool isActive)
        {
            _timerText.transform.parent.gameObject.SetActive(isActive);
        }

        private void DungeonEnded(IStageController stageController)
        {
            SwitchTimer(false);
            OnRetryButtonActivityChanged(ViewModel.IsRetryButtonActive.Value);
        }
    }
}