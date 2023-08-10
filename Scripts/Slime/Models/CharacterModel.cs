using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Configs.Characters;
using Slime.Data.Characters;
using Slime.Data.Enums;
using Slime.Data.Progress.Abstract;
using Slime.Exceptions;
using Slime.Factories;
using Slime.Models.Abstract;
using Utils;
using Utils.Extensions;

// ReSharper disable InconsistentNaming

namespace Slime.Models
{
    [UsedImplicitly]
    public class CharacterModel :
        BaseUpgradableModel<CharactersConfig, CharacterConfig, CharacterData>,
        ICharacterModel,
        IAuthorizedResourceUser
    {
        // public

        public string AuthorizationToken => Data.Constants.System.RESOURCE_TOKEN;
        
        #region ICharacterModel implementation

        public event Action OnEquip;

        public CharacterData GetEquipped()
        {
            return Get(GetEquippedCharacterID());
        }

        public void Equip(string ID)
        {
            SetEquippedCharacterID(ID);
        }

        public IUnitAvatar GetAvatar(string ID)
        {
            return _unitFactory.CreateCharacterAvatar(ID);
        }

        public int IndexOf(string id)
        {
            throw new NotImplementedException();
        }

        #endregion

        public bool IsEquipped(string ID)
        {
            return GetEquippedCharacterID() == ID;
        }

        // private

        private readonly UnitFactory _unitFactory;
        private readonly ISpritesModel _spritesModel;
        private readonly IResourcesModel _resourcesModel;

        private CharacterModel(
            IGameProgressModel progressModel,
            UnitFactory unitFactory,
            ISpritesModel spritesModel,
            IResourcesModel resourcesModel
        ) : base(progressModel)
        {
            _unitFactory = unitFactory;
            _spritesModel = spritesModel;
            _resourcesModel = resourcesModel;
        }

        #region BaseUpgradableModel overrides

        protected override void OnSubscribe()
        {
            _resourcesModel.OnChange += OnResourceChanged;
        }

        protected override void OnUnsubscribe()
        {
            _resourcesModel.OnChange -= OnResourceChanged;
        }

        protected override void OnProgressLoaded()
        {
            base.OnProgressLoaded();

            // NOTE: why?
            OnEquip?.Invoke();
        }

        protected override string ConfigPath => "Characters";

        protected override Dictionary<string, ProgressData> GetProgressData()
        {
            var data = GameData.CharacterData;
            if (data == null)
            {
                data = new Dictionary<string, ProgressData>();
                GameData.CharacterData = data;
            }

            return data;
        }

        protected override CharacterData PrepareData(CharacterData source)
        {
            var data = base.PrepareData(source);
            data.SetResourceQuantity(_resourcesModel.Get(EResource.CharacterUpgradeCurrency));
            data.IsEquipped = IsEquipped(data.ID);
            data.Sprite = _spritesModel.Get(data.ID);

            if (data.Abilities != null)
                foreach (var ability in data.Abilities)
                {
                    ability.Sprite = _spritesModel.Get(ability.GetSpriteId());
                }

            if (data.Skill != null)
            {
                data.Skill.Sprite = _spritesModel.Get(data.Skill.GetSpriteId());
            }

            return data;
        }
        

        public override void Upgrade(string id)
        {
            // check that is unlocked
            var item = Get(id);
            item.SetResourceQuantity(_resourcesModel.Get(EResource.CharacterUpgradeCurrency));
            if (!item.CanUpgrade)
            {
                Logger.Warning($"item {id} can't be upgraded (isUnlocked: {item.IsUnlocked}; progress {item.Progress})");
                return;
            }

            var quantity = item.UpgradeQuantity;
            
            // calculate internal level/quantity
            item.UpgradeOne();
            
            // update progress data
            var progressData = GetProgressData(id, true); 
            new ProgressData // unnecessary bravado
            {
                Level = item.Level,
            }.CopyFieldsTo(progressData);
            
            // remove resources
            if (!_resourcesModel.TrySpend(this, EResource.CharacterUpgradeCurrency, quantity))
            {
                throw new NotEnoughResourceException();
            }

            TriggerOnUpgrade(item);
            // not needed as we do UpdateState in subscription to IResourceModel
            //TriggerOnItemChange(item);
            //TriggerOnChange();
        }

        #endregion

        private void OnResourceChanged(EResource resource, float quantity)
        {
            if (resource == EResource.CharacterUpgradeCurrency)
            {
                UpdateState();
            }
        }

        private void SetEquippedCharacterID(string ID)
        {
            GameData.EquippedCharacterID = ID;
            UpdateState();
            
            Logger.Warning($"id: {GetEquippedCharacterID()}");
            OnEquip?.Invoke();
        }
        
        private string GetEquippedCharacterID()
        {
            return GameData.EquippedCharacterID;
        }
    }
}