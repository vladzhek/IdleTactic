using System.Linq;
using JetBrains.Annotations;
using Slime.Data.Abstract;
using Slime.Data.Constants;
using Slime.Data.Enums;
using Slime.UI.Common;
using Slime.UI.Common.Abilities;
using Slime.UI.Common.Equipment;
using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Logger = Utils.Logger;

namespace Slime.UI.Character.CharacterSelection
{
    public class CharacterSelectionView : View<CharacterSelectionViewModel>
    {
        public override UILayer Layer => UILayer.Middleground;
        
        [SerializeField] private TextMeshProUGUI _totalLevel;
        [SerializeField] private TextMeshProUGUI _characterGrade;
        [SerializeField] private TextMeshProUGUI _characterTitle;
        [SerializeField] private TextMeshProUGUI _characterLevel;
        
        [SerializeField] private EquipmentLayout _equipmentLayout;
        [SerializeField] private AbilitiesWidget _abilitiesWidget;
        [SerializeField] private EquipmentWidget _skillsWidget;
        
        [UsedImplicitly] [SerializeField] private Button _closeButton;
        [UsedImplicitly] [SerializeField] private GenericButton _upgradeButton;
        [UsedImplicitly] [SerializeField] private GenericButton _equipButton;
        
        #region View implementation
        
        protected override void Awake()
        {
            base.Awake();
            
            _abilitiesWidget.Clear();
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            UpdateState();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            ViewModel.OnChange += OnCharacterChanged;
            _equipmentLayout.OnSelect += OnSelectButtonClicked;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            ViewModel.OnChange -= OnCharacterChanged;
            _equipmentLayout.OnSelect -= OnSelectButtonClicked;
        }
        
        #endregion

        private void OnCharacterChanged()
        {
            // TODO: proper update, this one refreshes whole view
            UpdateState();
        }

        private void OnSelectButtonClicked(ILayoutElementData data)
        {
            ViewModel.Select(data.ID);
        }

        private void UpdateState()
        {
            // NOTE: this is called twice?
            //Logger.Log();
            
            var level = ViewModel.TotalLevel;
            _totalLevel.text = $"{level}";
            
            // selected item
            var character = ViewModel.GetSelected();
            if (character == null)
            {
                Logger.Warning($"selected item is null");
                // NOTE: hide selection block?
                return;
            }
            
            // character
            _characterGrade.color = character.Grade.GetColor();
            _characterGrade.text = character.Grade.ToString();
            _characterTitle.text = character.Title;
            _characterLevel.text = Strings.LEVEL.Resolve(character.Level);
            
            var isEquipped = ViewModel.IsEquipped(character.ID);
            _equipButton.Interactable = !isEquipped;
            _equipButton.Title = isEquipped ? Strings.EQUIPPED_TEXT : Strings.EQUIP_TEXT;
            
            // all items
            var items = ViewModel.Get();
            _equipmentLayout.SetData(from item in items select new GridLayoutData(item));

            // abilities
            var abilities = character.Abilities;
            _abilitiesWidget.gameObject.SetActive(abilities != null);
            if (abilities != null)
            {
                _abilitiesWidget.SetData(from ability in abilities 
                    orderby ability.UnlocksAtCharacterLevel
                    select new AbilityLayoutData(
                        ability.Sprite,
                        character.Level >= ability.UnlocksAtCharacterLevel
                        ));
            }

            // unique skill
            var skill = character.Skill;
            _skillsWidget.gameObject.SetActive(skill != null);
            if (skill != null)
            {
                _skillsWidget.SetData(new GridLayoutData(skill)); 
            }
            
            // upgrade
            _upgradeButton.Title = Strings.UPGRADE_BUTTON_TITLE.Resolve(character.Quantity, character.UpgradeQuantity);
            _upgradeButton.Interactable = character.CanUpgrade;

            
            RequestRebuild();
        }
        
        private void RequestRebuild()
        {
            Invoke(nameof(Rebuild), .1f);
        }
        
        private void Rebuild()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_skillsWidget.transform as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_skillsWidget.transform.parent as RectTransform);
        }
    }
}