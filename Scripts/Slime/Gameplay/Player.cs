using System;
using System.Linq;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Slime.Services;
using Utils;
using Zenject;
using Attribute = Slime.AbstractLayer.Stats.Attribute;

namespace Slime.Gameplay
{
    public class Player : IPlayer, IInitializable, IDisposable
    {
        // public
        
        #region IPlayer implementation

        public IUnit Unit { get; private set; }

        public void SetUnit(IUnit unit)
        {
            Unit?.Dispose();
            Unit = unit;
            
            UpdateState();
        }

        #endregion
        
        #region IInitializable, IDisposable
        
        public void Initialize()
        {
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        #endregion
        
        // private

        private readonly ICameraService _cameraService;
        private readonly ICharacterModel _characterModel;
        private readonly IParametersModel _parametersModel;
        
        private Player(
            ICameraService cameraService,
            ICharacterModel characterModel,
            IParametersModel parametersModel
            )
        {
            _cameraService = cameraService;
            _characterModel = characterModel;
            _parametersModel = parametersModel;
        }

        private void Subscribe()
        {
            _characterModel.OnEquip += OnCharacterEquipped;
            _parametersModel.OnChange += OnParametersChanged;
        }
        
        private void Unsubscribe()
        {
            _characterModel.OnEquip -= OnCharacterEquipped;
            _parametersModel.OnChange += OnParametersChanged;
        }

        private void OnCharacterEquipped()
        {
            UpdateCharacter();
        }
        
        private void OnParametersChanged()
        {
            UpdateParameters();
        }

        private void UpdateState()
        {
            UpdateCharacter();
            UpdateParameters();
        }
        
        private void UpdateCharacter()
        {
            if (Unit == null)
            {
                //Logger.Warning($"unit is not ready");
                return;
            }
            
            // TODO: implement
            /*if (!_characterModel.IsLoaded)
            {
                Logger.Warning($"character model is not ready");
                return;
            }*/

            // replace prefab
            var id = _characterModel.GetEquipped().ID;
            var avatar = _characterModel.GetAvatar(id);
            Unit.InjectAvatar(avatar);
            
            // set position
            Unit.SyncPosition();
            
            // set as camera target
            _cameraService.SetMainCameraTarget(avatar.Object.transform);
        }

        private void UpdateParameters()
        {
            //Logger.Warning($"parameters: {_parametersModel.Get().Count}");
            
            Unit?.UpdateParameters(
                from parameter in _parametersModel.Get() 
                select new Attribute(parameter.Key.ToID(), parameter.Value));
        }
    }
}