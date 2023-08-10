using JetBrains.Annotations;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Logger = Utils.Logger;

namespace Slime.UI.Boosters
{
    public class BoostersView : View<BoostersViewModel>
    {
        [SerializeField] private BoostersLayout _boostersLayout;
        [SerializeField, UsedImplicitly] private Button _closeButton;
        public override UILayer Layer => UILayer.Overlay;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            UpdateState();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            _boostersLayout.OnSelect += OnSelected;
            ViewModel.OnChange += OnBoostersChanged;
            ViewModel.OnActivated += UpdateBoosterLayoutElement;
            ViewModel.OnDeactivated += OnDeactivated;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

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

        private void SetTimerForLayoutElement(string id)
        {
            var timer = ViewModel.GetTimer(id);
            var layoutElement = _boostersLayout.Elements.Find(x => x.Data.ID == id);

            if (layoutElement.Data.IsActive)
            {
                layoutElement.SetTimer(timer);
            }
        }

        private void OnDeactivated(string id)
        {
            UpdateBoosterLayoutElement(id);
        }
    }
}