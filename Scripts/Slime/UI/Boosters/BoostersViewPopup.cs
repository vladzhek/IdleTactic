using JetBrains.Annotations;
using Slime.UI.Popups;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

namespace Slime.UI.Boosters
{
    public class BoostersViewPopup : View<BoostersViewModel>
    {
        [SerializeField] private PopupWidget _popupWidget;
        [SerializeField] private BoostersLayout _boostersLayout;
        public override UILayer Layer => UILayer.Overlay;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _popupWidget.SetTile("BOOSTERS");
            UpdateState();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            _popupWidget.OnCloseButtonClick += OnButtonClose;
            _boostersLayout.OnSelect += OnSelected;
            ViewModel.OnChange += OnBoostersChanged;
            ViewModel.OnActivated += UpdateBoosterLayoutElement;
            ViewModel.OnDeactivated += OnDeactivated;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            _popupWidget.OnCloseButtonClick -= OnButtonClose;
            _boostersLayout.OnSelect -= OnSelected;
            ViewModel.OnChange -= OnBoostersChanged;
            ViewModel.OnActivated -= UpdateBoosterLayoutElement;
            ViewModel.OnDeactivated -= OnDeactivated;
        }

        private void OnBoostersChanged()
        {
            UpdateState();
        }
        
        private void UpdateState()
        {
            Logger.Warning($"values: {ViewModel.Data.Values.Count}");
            _boostersLayout.SetData(ViewModel.Data.Values);

            InitializeTimerForLayoutElement();
        }

        private void InitializeTimerForLayoutElement()
        {
            foreach (var element in _boostersLayout.Elements)
            {
                if (element.Data.IsActive)
                {
                    SetTimerForLayoutElement(element.Data.ID);
                }
            }
        }

        private void UpdateBoosterLayoutElement(string id)
        {
            for (var i = 0; i < _boostersLayout.Elements.Count; i++)
            {
                var layoutElement = _boostersLayout.Elements[i];

                var viewData = ViewModel.Get(id);

                if (layoutElement.Data.ID == id)
                {
                    if (layoutElement != null)
                    {
                        _boostersLayout.SetData(i, viewData);

                        if (layoutElement.Data.IsActive)
                        {
                            SetTimerForLayoutElement(layoutElement.Data.ID);
                        }

                        return;
                    }
                }
            }
        }

        private void OnSelected(BoosterLayoutElementViewData viewData)
        {
            ViewModel.OnSelected(viewData);
        }

        private void SetTimerForLayoutElement(string ID)
        {
            var timer = ViewModel.GetTimer(ID);
            var layoutElement = _boostersLayout.Elements.Find(x => x.Data.ID == ID);

            if (layoutElement.Data.IsActive)
            {
                layoutElement.SetTimer(timer);
            }
        }

        private void OnDeactivated(string id)
        {
            UpdateBoosterLayoutElement(id);
        }
        
        private void OnButtonClose()
        {
            ViewModel.OnCloseButtonClicked();
        }
    }
}