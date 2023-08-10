using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Data.Characters;
using Slime.UI.Footer.Character;
using UI.Base.MVVM;
using UnityEngine;
using Utils;
using Logger = Utils.Logger;

namespace Slime.UI.Character.CharacterSelection
{
    public class CharacterSelectionViewModel : ViewModel
    {
        // dependencies
        private readonly RenderTextureController _renderTextureController;
        private readonly UIManager _uiManager;
        private readonly ICharacterModel _characterModel;
        
        private CharacterSelectionViewModel(
            RenderTextureController renderTextureController,
            UIManager uiManager, 
            ICharacterModel characterModel)
        {
            _renderTextureController = renderTextureController;
            _uiManager = uiManager;
            _characterModel = characterModel;
        }
        
        public int TotalLevel => (from character in _characterModel.Get() select character.Level).Sum();
        
        public event Action OnChange;
        
        public IEnumerable<CharacterData> Get()
        {
            return _characterModel.Get(); 
        }

        public CharacterData GetSelected()
        {
            _selected ??= GetEquipped();
            _selected ??= Get().FirstOrDefault();
            
            return _selected;
        }
        
        public void Select(string ID)
        {
            _selected = _characterModel.Get(ID);
            
            // character representation
            var character = _characterModel.GetAvatar(ID);
            _renderTextureController.RenderAsCharacter(character.Object.Transform);
            
            OnChange?.Invoke();
        }

        public bool IsEquipped(string ID)
        {
            return _characterModel.IsEquipped(ID);
        }
        
        #region ViewModel implementation

        public override void OnSubscribe()
        {
            base.OnSubscribe();

            _characterModel.OnChange += OnChanged;
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            
            _characterModel.OnChange -= OnChanged;
        }
        
        #endregion

        #region View events

        [UsedImplicitly]
        private void OnUpgradeButtonClicked()
        {
            var selected = GetSelected();
            if (selected == null)
            {
                Logger.Warning("no element is selected");
                return;
            }
            
            _characterModel.Upgrade(selected.ID);
        }

        [UsedImplicitly]
        private void OnEquipButtonClicked()
        {
            var selected = GetSelected();
            if (selected == null)
            {
                Logger.Warning("no element is selected");
                return;
            }

            Equip(selected.ID);
        }

        [UsedImplicitly]
        private void OnCloseButtonClicked()
        {
            // TODO: implement closing with restoration of last opened view
            //_uiManager.Close<CharacterSelectionView>();
            _uiManager.Open<CharacterTabbarView>();
        }

        #endregion
        
        private CharacterData _selected;

        private CharacterData GetEquipped()
        {
            return _characterModel.GetEquipped();
        }

        private void Equip(string ID)
        {
            _characterModel.Equip(ID);
        }

        private void OnChanged()
        {
            //Logger.Log($"id: {GetEquipped().ID}");
            
            OnChange?.Invoke();
        }
    }
}