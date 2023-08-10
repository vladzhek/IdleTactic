using System.Linq;
using Slime.AbstractLayer.Services;
using Slime.Data.Constants;
using Slime.Data.Enums;
using Slime.UI.Abstract;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Footer.Tabbar
{
    // TODO: replace background with header prefab
    // TODO: add section title
    public class FooterView : View<FooterViewModel>
    {
        [SerializeField] private BaseLayoutWidget<FooterTab, FooterLayoutData> _tabbarWidget;
        [SerializeField] private GameObject _lockScreen;

        public override UILayer Layer => UILayer.Foreground;

        protected override void OnEnable()
        {
            base.OnEnable();
            _tabbarWidget.SetData(ViewModel.Data);
            TutorialChanged(ViewModel.GetCurrentTutorialStage());

            _tabbarWidget.OnSelect += OnElementSelected;
            ViewModel.OnTabChange += OnTabChanged;
            ViewModel.OnStageController += OnStageController;
            ViewModel.TutorialChanged += TutorialChanged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _tabbarWidget.OnSelect -= OnElementSelected;
            ViewModel.OnTabChange -= OnTabChanged;
            ViewModel.OnStageController -= OnStageController;
            ViewModel.TutorialChanged -= TutorialChanged;
        }

        private void TutorialChanged(ETutorialStage stage)
        {
            switch (stage)
            {
                case ETutorialStage.Welcome:
                    SetTabLocked(-1);
                    return;
                case ETutorialStage.Attribute: 
                    SetTabLocked(-1);
                    return;
                case ETutorialStage.StageReplay: 
                    SetTabLocked(-1);
                    return;
                case ETutorialStage.Summon:
                    _tabbarWidget.Elements[Values.FOOTER_TAB_STORE].SetTutorialActive(true);
                    SetTabLocked(-1);
                    SetTabLocked(Values.FOOTER_TAB_STORE, true);
                    return;
                case ETutorialStage.Inventory:
                    _tabbarWidget.Elements[Values.FOOTER_TAB_CHARACTER].SetTutorialActive(true);
                    SetTabLocked(-1);
                    SetTabLocked(Values.FOOTER_TAB_CHARACTER, true);
                    return;
                case ETutorialStage.Complete:
                    SetTabLocked(-1, true);
                    SetTabLocked(Values.FOOTER_TAB_CASTLE);
                    break;
            }
        }

        private void SetTabLocked(int index, bool isUnlocked = false)
        {
            for (var i = 0; i < _tabbarWidget.Elements.Count; i++)
            {
                if (index != -1 && index != i)
                {
                    continue;
                }
                
                var tab = _tabbarWidget.Elements[i];
                tab.SetSelectable(isUnlocked);
            }
        }

        private void OnStageController(IStageController stageController)
        {
            _lockScreen.SetActive(stageController.CurrentStage.StageConfig.StageType != EStageType.Default);
        }

        private void OnElementSelected(FooterLayoutData data)
        {
            //Logger.Log($"{index}");

            ViewModel.SetActiveIndex(ViewModel.Data.ToList().IndexOf(data));
        }

        private void OnTabChanged(int index, FooterLayoutData data)
        {
            //Logger.Log($"{index}");

            _tabbarWidget.SetData(index, data);
        }
    }
}