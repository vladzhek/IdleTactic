using System.Diagnostics.CodeAnalysis;
using Slime.Data.Enums;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.UI.Attributes
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AttributesView : View<AttributesViewModel>
    {
        [SerializeField] private TMP_Text _dpsText;
        [SerializeField] private AttributesLayout _attributesLayout;

        #region View overrides

        public override UILayer Layer => UILayer.Middleground;

        protected override void OnEnable()
        {
			base.OnEnable();
			
			UpdateState();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            ViewModel.OnDPSChange += OnDPSChanged;
            ViewModel.OnChange += OnAttributeChanged;
            _attributesLayout.OnSelect += OnElementSelected;
            ViewModel.OnTutorialActive += OnTutorialActivated;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            ViewModel.OnDPSChange -= OnDPSChanged;
            ViewModel.OnChange -= OnAttributeChanged;
            _attributesLayout.OnSelect -= OnElementSelected;
            ViewModel.OnTutorialActive -= OnTutorialActivated;
        }

        #endregion

        private void OnDPSChanged()
        {
            UpdateDPS();
        }

        private void OnAttributeChanged()
        {
            UpdateAttributes();
        }

        private void UpdateState()
        {
            UpdateDPS();
            UpdateAttributes();
        }

        private void UpdateDPS()
        {
            _dpsText.text = ViewModel.DPS;
        }

        private void UpdateAttributes()
        {
            _attributesLayout.SetData(ViewModel.Data);

            if (ViewModel.IsTutorialActive)
            {
                OnTutorialActivated(true);
            }
        }

        private void OnElementSelected(AttributeData data)
        {
            ViewModel.Upgrade(data.ID);

            OnTutorialActivated(false);
        }

        private void OnTutorialActivated(bool isActive)
        {
            _attributesLayout.Elements
                .Find(x => x.Data.ID == EAttribute.Damage.ToString())?
                .SetActiveTutorialFinger(isActive);
        }
    }
}