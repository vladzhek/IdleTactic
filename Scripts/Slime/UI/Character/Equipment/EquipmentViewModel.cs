using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.Characters;
using Slime.Data.Enums;
using Slime.Data.Inventory;
using Slime.UI.Character.CharacterSelection;
using Slime.UI.Inventory;
using UI.Base.MVVM;
using UnityEngine;
using Utils;

namespace Slime.UI.Character.Equipment
{
    public class EquipmentViewModel : ViewModel
    {
        // dependencies
        private readonly RenderTextureController _renderTextureController;
        private readonly UIManager _uiManager;
        private readonly ICharacterModel _characterModel;
        private readonly IInventoryModel _inventoryModel;
        private readonly IInventoryUIModel _inventoryUIModel;
        private readonly ITutorialModel _tutorialModel;
        
        private EquipmentViewModel(
            RenderTextureController renderTextureController,
            UIManager uiManager, 
            ICharacterModel characterModel,
            IInventoryModel inventoryModel,
            IInventoryUIModel inventoryUIModel,
            ITutorialModel tutorialModel
            )
        {
            _renderTextureController = renderTextureController;
            _uiManager = uiManager;
            _characterModel = characterModel;
            _inventoryModel = inventoryModel;
            _inventoryUIModel = inventoryUIModel;
            _tutorialModel = tutorialModel;
        }

        public event Action OnCharacterChange;
        public event Action OnInventoryChange;

        public event Action<bool> OnTutorialChange; 

        public CharacterData Character => _characterModel.GetEquipped();

        public IEnumerable<InventoryData> Inventory => _inventoryModel.GetEquipped();
        public int TotalLevel => (from character in _characterModel.Get() select character.Level).Sum();
        
        #region ViewModel implementation

        public override void OnSubscribe()
        {
            base.OnSubscribe();
            
            _characterModel.OnChange += OnCharacterChanged;
            _inventoryModel.OnChange += OnInventoryChanged;
            _tutorialModel.OnChange += OnTutorialChanged;
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _characterModel.OnChange -= OnCharacterChanged;
            _inventoryModel.OnChange -= OnInventoryChanged;
            _tutorialModel.OnChange -= OnTutorialChanged;
        }

        public override void OnEnable()
        {
            // TODO: check why this is called twice
            // TODO: separate render textures? access to RenderTextureController only through models?
            // character representation
            var id = _characterModel.GetEquipped()?.ID;
            if (!string.IsNullOrEmpty(id)) 
            {
                var character = _characterModel.GetAvatar(id);
                _renderTextureController.RenderAsCharacter(character.Object.Transform);
            }

            OnTutorialChanged(_tutorialModel.Stage);
        }
        
        #endregion
        
        #region View events
        
        [UsedImplicitly]
        private void OnWeaponButtonClicked()
        {
            OpenInventoryView(EInventoryType.Weapon);
        }
        
        [UsedImplicitly]
        private void OnArmorButtonClicked()
        {
            OpenInventoryView(EInventoryType.Armor);
        }
        
        [UsedImplicitly]
        private void OnAccessoryButtonClicked()
        {
            OpenInventoryView(EInventoryType.Accessory);
        }
        
        [UsedImplicitly]
        private void OnUpgradeButtonClicked()
        {
            _characterModel.Upgrade(Character.ID);
        }
        
        [UsedImplicitly]
        private void OnSelectButtonClicked()
        {
            _uiManager.Open<CharacterSelectionView>();
        }
        
        #endregion
        
        private void OnCharacterChanged()
        {
            OnCharacterChange?.Invoke();
        }

        private void OnInventoryChanged()
        {
            OnInventoryChange?.Invoke();
        }

        private void OnTutorialChanged(ETutorialStage stage)
        {
            OnTutorialChange?.Invoke(_tutorialModel.Stage == ETutorialStage.Inventory);
        }
        
        private void OpenInventoryView(EInventoryType type)
        {
            _inventoryUIModel.SetInventoryType(type);
            _uiManager.Open<InventoryView>();
        }
    }
}