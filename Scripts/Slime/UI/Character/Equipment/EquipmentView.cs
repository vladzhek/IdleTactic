using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

namespace Slime.UI.Character.Equipment
{
    public class EquipmentView : View<EquipmentViewModel>
    {
        public override UILayer Layer => UILayer.CharacterTabbar;

        // character

        [SerializeField] private TextMeshProUGUI _totalLevel;
        [SerializeField] private TextMeshProUGUI _characterGrade;
        [SerializeField] private TextMeshProUGUI _characterTitle;
        [SerializeField] private TextMeshProUGUI _characterLevel;
        
        [SerializeField] private AbilitiesWidget _abilitiesWidget;
        [SerializeField] private EquipmentWidget _skillsWidget;
        
        [SerializeField, UsedImplicitly] private GenericButton _upgradeButton;
        [SerializeField, UsedImplicitly] private GenericButton _selectButton;

        [SerializeField] private RectTransform _tutorialFinger;

        // equipment

        [SerializeField, UsedImplicitly] private EquipmentButton _weaponButton;
        [SerializeField, UsedImplicitly] private EquipmentButton _armorButton;
        [SerializeField, UsedImplicitly] private EquipmentButton _accessoryButton;

        private Dictionary<EInventoryType, EquipmentButton> _buttonsByType;
        
        #region View implementation

        protected override void Awake()
        {
            base.Awake();
            
            _abilitiesWidget.Clear();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            UpdateCharacter();
            UpdateInventory();
        }
        
        protected override void OnSubscribe()
        {
            base.OnSubscribe();

            ViewModel.OnCharacterChange += OnCharacterChanged;
            ViewModel.OnInventoryChange += OnInventoryChanged;
            ViewModel.OnTutorialChange += OnTutorialChanged;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            ViewModel.OnCharacterChange -= OnCharacterChanged;
            ViewModel.OnInventoryChange -= OnInventoryChanged;
            ViewModel.OnTutorialChange -= OnTutorialChanged;
        }
        
        #endregion
        
        private void UpdateCharacter()
        {
            // NOTE: this is called twice?
            //Logger.Warning();
            
            var level = ViewModel.TotalLevel;
            _totalLevel.text = $"{level}";
            
            // character
            var character = ViewModel.Character;
            _characterGrade.color = character.Grade.GetColor();
            _characterGrade.text = character.Grade.ToString();
            _characterTitle.text = character.Title;
            _characterLevel.text = Strings.LEVEL.Resolve(character.Level);

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
        
        private void UpdateInventory()
        {
            _buttonsByType ??= new Dictionary<EInventoryType, EquipmentButton>
            {
                { EInventoryType.Weapon, _weaponButton },
                { EInventoryType.Armor, _armorButton },
                { EInventoryType.Accessory, _accessoryButton }
            };
            
            var data = ViewModel.Inventory.ToDictionary(x => x.Type);
            foreach (var (type, button) in _buttonsByType)
            {
                var item = data.GetValueOrDefault(type);
                button.Level = item?.Level ?? 0;
                button.Sprite = item?.Sprite;
                button.RarityColor = item?.Rarity.ToColor() ?? Color.white;
                button.Title = !string.IsNullOrEmpty(item?.Title) ? item.Title : type.GetTitle();
            }
        }

        private void OnCharacterChanged()
        {
            UpdateCharacter();
        }
        
        private void OnInventoryChanged()
        {
            UpdateInventory();
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

        private void OnTutorialChanged(bool isActive)
        {
            _tutorialFinger.gameObject.SetActive(isActive);
        }
    }
}