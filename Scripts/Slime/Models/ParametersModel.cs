using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Equipment;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Utils.Extensions;
using Zenject;

namespace Slime.Models
{
    public class ParametersModel : IParametersModel, IInitializable, IDisposable
    {
        // public
        
        #region IParametersModel implementation

        public event Action OnChange;

        public IDictionary<EAttribute, float> Get()
        {
            return _parameters;
        }

        public float Get(EAttribute attribute)
        {
            return _parameters.GetValueOrDefault(attribute);
        }

        #endregion

        #region IInitializable, IDisposable
        
        public void Initialize()
        {
            Subscribe();
            UpdateState();
        }

        public void Dispose()
        {
            Unsubscribe();
        }
        
        #endregion
        
        // private

        private readonly IAttributesModel _attributesModel;
        private readonly IEnumerable<IEquipmentModel> _models;

        private ParametersModel(
            IAttributesModel attributesModel,
            IEnumerable<IEquipmentModel> models)
        {
            _attributesModel = attributesModel;
            _models = models;
        }

        private void Subscribe()
        {
            _attributesModel.OnChange += OnChanged;
            
            foreach (var model in _models)
            {
                model.OnChange += OnChanged;
            }
        }
        
        private void Unsubscribe()
        {
            _attributesModel.OnChange -= OnChanged;
            
            foreach (var model in _models)
            {
                model.OnChange -= OnChanged;
            }
        }

        private void OnChanged()
        {
            //Logger.Warning();
            
            UpdateState();
        }

        private readonly Dictionary<EAttribute, float> _parameters = new();

        private void UpdateState()
        {
            _parameters.Clear();
            
            // equipment
            foreach (var model in _models)
            {
                foreach (var (attribute, value) in model.ParameterModifiers)
                {
                    _parameters.Increment(attribute, value);
                }
            }

            /*
            Logger.Warning("--- Attributes ---");
            foreach (var data in _attributesModel.Get())
            {
                Logger.Log($"id: {data.ID}; value: {data.ActiveValue}");
            }
            
            Logger.Warning("--- Modifiers ---");
            foreach (var (attribute, value) in _parameters)
            {
                Logger.Log($"id: {attribute}; value: {value}");
            }
            */

            // attributes
            foreach (var data in _attributesModel.Get())
            {
                var attribute = data.Type;
                _parameters.Increment(attribute, 1);
                _parameters[attribute] *= data.ActiveValue;
            }
            
           /*
            Logger.Warning("--- Parameters ---");
            foreach (var (attribute, value) in _parameters)
            {
                Logger.Log($"id: {attribute}; value: {value}");
            }
            */
            
            OnChange?.Invoke();
        }
    }
}