using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using UI.Base.MVVM;
using Utils.Extensions;

namespace Slime.UI.Attributes
{
    [UsedImplicitly]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AttributesViewModel : ViewModel
    {
        #region ViewModel overrides
        
        public override void OnEnable()
        {
            base.OnEnable();
            
            CalculateDPS();
        }

        public override void OnSubscribe()
        {
            base.OnSubscribe();
            
            _attributesModel.OnChange += OnAttributesChanged;
            _parametersModel.OnChange += OnParametersChanged;
            _tutorialModel.OnChange += TutorialStageChanged;
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _attributesModel.OnChange -= OnAttributesChanged;
            _parametersModel.OnChange -= OnParametersChanged;
            _tutorialModel.OnChange -= TutorialStageChanged;
        }

        #endregion
        
        public event Action OnDPSChange;
        public event Action OnChange;
        public event Action<bool> OnTutorialActive;
        public bool IsTutorialActive => _tutorialModel.Stage == ETutorialStage.Attribute;

        public IEnumerable<AttributeData> Data =>
            from data in _attributesModel.Get()
            where data.IsUpgradable
            select new AttributeData(data.Sprite)
            {
                ID = data.ID,
                Title = data.Title,
                Level = data.Level.ToMetricPrefixString(),
                Value = data.ActiveValue.ToMetricPrefixString(),
                Cost = data.UpgradeCost.ToMetricPrefixString(),
                CanUpgrade = data.CanUpgrade
            };

        public void Upgrade(string id)
        {
            _attributesModel.Upgrade(id);
        }

        public string DPS { get; private set; }

        // private
        
        private readonly IAttributesModel _attributesModel;
        private readonly IParametersModel _parametersModel;
        private readonly ITutorialModel _tutorialModel;
        private IResourcesModel _resourcesModel;
        
        private AttributesViewModel(
            IAttributesModel attributesModel,
            IParametersModel parametersModel,
            ITutorialModel tutorialModel)
        {
            _attributesModel = attributesModel;
            _parametersModel = parametersModel;
            _tutorialModel = tutorialModel;
        }

        private void OnAttributesChanged()
        {
            OnChange?.Invoke();
        }
        
        private void OnParametersChanged()
        {
           CalculateDPS();
        }

        private void CalculateDPS()
        {
            var damage = _parametersModel.Get(EAttribute.Damage);
            var attackSpeed = _parametersModel.Get(EAttribute.AttackSpeed);

            DPS = (damage * attackSpeed).ToMetricPrefixString();
            
            //Logger.Warning($"damage: {damage}; attackSpeed: {attackSpeed}; dps: {DPS}");

            // TODO: chance attacks
            
            OnDPSChange?.Invoke();
        }

        private void TutorialStageChanged(ETutorialStage stage)
        {
            if (stage == ETutorialStage.Attribute)
            {
                OnTutorialActive?.Invoke(true);
            }
        }
    }
}