using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Slime.Data.Enums;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Store.Summon
{
    public class SummonView : View<SummonViewModel>
    {
        public override UILayer Layer => UILayer.StoreTabbar;

        [SerializeField] private LayoutElementConfig[] _layoutElementConfigs;
        private Dictionary<ESummonType, SummonLayoutElement> _layoutElements;

        protected override void OnEnable()
        {
            base.OnEnable();

            UpdateState();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            ViewModel.OnChange += OnChanged;
            ViewModel.OnTimerChange += OnTimerChanged;
            foreach (var (_, element) in LayoutElements)
            {
                element.OnInfoButtonClick += OnInfoButtonClicked;
                element.OnAdButtonClick += OnAdButtonClicked;
                element.OnLowQuantitySummonButtonClick += OnLowQuantitySummonButtonClicked;
                element.OnHighQuantitySummonButtonClick += OnHighQuantitySummonButtonClicked;
            }

            SetTutorialActive();
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            ViewModel.OnChange -= OnChanged;
            ViewModel.OnTimerChange -= OnTimerChanged;
            foreach (var (_, element) in LayoutElements)
            {
                element.OnInfoButtonClick -= OnInfoButtonClicked;
                element.OnAdButtonClick -= OnAdButtonClicked;
                element.OnLowQuantitySummonButtonClick -= OnLowQuantitySummonButtonClicked;
                element.OnHighQuantitySummonButtonClick -= OnHighQuantitySummonButtonClicked;
            }
        }

        private Dictionary<ESummonType, SummonLayoutElement> LayoutElements { get
            {
                return _layoutElements ??= _layoutElementConfigs.ToDictionary(
                    config => config.Type,
                    config => config.Element);
            }
        }
        
        private void OnChanged(ESummonType type, SummonLayoutElementData data)
        {
            SetData(type, data);
        }
        
        private void OnTimerChanged(string timerText)
        {
            foreach (var (_, element) in LayoutElements)
            {
                element.SetTimerText(timerText);
            }
        }

        private void UpdateState()
        {
            foreach (var (type, data) in ViewModel.Data)
            {
                SetData(type, data);
            }
        }

        private void SetData(ESummonType type, SummonLayoutElementData data)
        {
            //Logger.Log($"type: {type}; data: {data}");
            
            if (!LayoutElements.TryGetValue(type, out var element))
            {
                throw new Exception($"no {type} layout element");
            }
            
            element.SetData(data);
        }
        
        private void OnInfoButtonClicked(ESummonType type)
        {
            ViewModel.OnInfoButtonClicked(type);
        }
        
        private void OnAdButtonClicked(ESummonType type)
        {
            ViewModel.OnAdButtonClicked(type);
        }
        
        private void OnLowQuantitySummonButtonClicked(ESummonType type)
        {
            ViewModel.OnLowQuantitySummonButtonClicked(type);
            
            SetTutorialActive();
        }
        
        private void OnHighQuantitySummonButtonClicked(ESummonType type)
        {
            ViewModel.OnHighQuantitySummonButtonClicked(type);
        }

        private void SetTutorialActive()
        {
            LayoutElements[ESummonType.Gear].SetTutorialActive(ViewModel.GetTutorialActive());
        }
    }

    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class LayoutElementConfig
    {
        public ESummonType Type;
        public SummonLayoutElement Element;
    } 
}