using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ModestTree;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.StateMachine;
using Slime.AbstractLayer.Stats;
using Slime.Data.IDs;
using UnityEngine;
using UnityEngine.Rendering;
using Utils.Promises;
using Attribute = Slime.AbstractLayer.Stats.Attribute;
using Logger = Utils.Logger;

namespace Slime.Gameplay.Battle.Units
{
    public abstract class UnitBase : IUnit
    {
        public event Action<IUnit> Die;
        public event Action<IUnit> PositionChanged;
        public event Action<IUnit, float> HealthChanged;

        public string ID { get; }

        private string _status;
        private float _regenerationTime;

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                Avatar.DisplayStatus(_status);
            }
        }

        public abstract ESide Side { get; }

        public IUnitBehaviour Behaviour { get; private set; }
        public IUnitAvatar Avatar { get; private set; }
        public IUnit Target { get; private set; }
        public Vector3 Position { get; private set; }
        public virtual Vector3 HealthbarPosition => Position + Vector3.up * 4f;
        public Quaternion Rotation { get; private set; }
        public bool IsActive { get; private set; }
        
        public int Index { get; set; }

        private readonly Dictionary<string, Attribute> _attributes = new();
        private readonly Dictionary<string, Parameter> _parameters = new();

        public UnitBase(string id)
        {
            ID = id;
        }

        public void InitializeParameters(Dictionary<string, Attribute> attributes)
        {
            foreach (var (id, attribute) in attributes)
            {
                _attributes.Add(id, attribute);
            }

            var health = GetAttribute(AttributeIDs.Health);
            if (health == null)
            {
                Logger.Error($"no attribute {AttributeIDs.Health} for unit {ID}");
            }

            var healthParameter = new Parameter(health);
            healthParameter.Changed += OnHealthChanged;
            _parameters.Add(AttributeIDs.Health, healthParameter);

            SetActive(true);
        }
        
        public void UpdateParameters(IEnumerable<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                var id = attribute.ID;
                
                if (!_attributes.TryAdd(id, attribute))
                {
                    _attributes[id].SetValue(attribute.BaseValue);
                }
                else if (id == AttributeIDs.Health)
                {
                    var parameter = new Parameter(attribute);
                    parameter.Changed += OnHealthChanged;
                    _parameters.Add(id, parameter);
                }
            }

            SetActive(true);
        }

        public void InjectAvatar(IUnitAvatar avatar)
        {
            Avatar?.Release();

            Avatar = avatar;
        }

        public void SetBehaviour(IUnitBehaviour unitBehaviour)
        {
            Behaviour = unitBehaviour;
            Behaviour.SetUnit(this);
        }

        public void DestroyUnit()
        {
            IsActive = false;
            Avatar.SetActiveUnit(false);
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive && GetParameter(AttributeIDs.Health)?.Value > 0;
        }

        public virtual void OnUpdate(float deltaTime)
        {
            if (!IsActive) return;

            // not a problem if projectile is not assigned
            /*if (Avatar.ProjectileOrigin == Vector3.zero)
            {
                Logger.Warning($"type: {GetType()}; unit: {Avatar.Object}; origin: {Avatar.ProjectileOrigin}");
            }*/

            Behaviour.OnUpdate(deltaTime);

            var health = GetParameter(AttributeIDs.Health);
            var regeneration = GetAttribute(AttributeIDs.RegenerationRate);
            if (health == null || regeneration == null) return;
            
            _regenerationTime += deltaTime;
            if (_regenerationTime < regeneration.FinalValue) return;
            
            health.Change(regeneration.FinalValue).Execute();
            _regenerationTime = 0;
        }

        public void SetTarget(IUnit unit)
        {
            if (Target != null)
            {
                Target.Die -= OnTargetDies;
            }

            Target = unit;

            if (unit != null)
            {
                //LogsWrap.Log($"{Avatar.Name} target {unit.Avatar.Name}");
                Target.Die += OnTargetDies;
            }
        }

        public void SetPosition(Vector3 position, Quaternion rotation = default)
        {
            Position = position;
            Rotation = rotation;
            
            Avatar.SetPosition(position);
            if (rotation != default)
            {
                Avatar.SetRotation(rotation);
            }

            PositionChanged?.Invoke(this);
        }

        public void SyncPosition()
        {
            SetPosition(Position, Rotation);
        }

        public Attribute GetAttribute(string attributeID)
        {
            if (_attributes.TryGetValue(attributeID, out var attribute)) return attribute;
            Logger.Warning($"No attribute {attributeID}");
            return null;
        }

        public Parameter GetParameter(string parameterID)
        {
            if (_parameters == null)
            {
                Logger.Error($"no parameters");
                return null;
            }

            if (!_parameters.TryGetValue(parameterID, out var parameter))
            {
                Logger.Warning($"no parameter {parameterID} for unit {ID}");
                return null;
            }

            return parameter;
        }

        public void ResetParameters()
        {
            foreach (var parameter in _parameters)
            {
                parameter.Value.Reset();
            }
        }

        public Promise ApplyDamage(float damage)
        {
            if (!_attributes.TryGetValue(AttributeIDs.Health, out var healthAttribute))
            {
                UnityEngine.Debug.LogError($"No attribute {AttributeIDs.Health} for {ID}");
                return null;
            }

            if (!_parameters.TryGetValue(AttributeIDs.Health, out var health))
            {
                health = new Parameter(GetAttribute(AttributeIDs.Health));
                _parameters.Add(AttributeIDs.Health, health);
            }

            return health.Change(-damage);
        }

        public void Reset()
        {
            Target = null;
            ResetParameters();
            Avatar.ResetState();
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public void Dispose()
        {
            GetParameter(AttributeIDs.Health).Changed -= OnHealthChanged;

            Behaviour.Dispose();
            Avatar.Release();

            Behaviour = null;
            Avatar = null;

            foreach (var parameter in _parameters)
            {
                parameter.Value.Dispose();
            }

            _parameters.Clear();
            _attributes.Clear();

            Target = null;
        }

        private void OnHealthChanged(Parameter health, float delta)
        {
            Avatar.DisplayHealth(health.Value);
            HealthChanged?.Invoke(this, delta);

            if (health.Value <= 0 && IsActive)
            {
                SetActive(false);

                OnDeath();
                Die?.Invoke(this);
            }
        }

        protected abstract void OnDeath();

        private void OnTargetDies(IUnit target)
        {
            target.Die -= OnTargetDies;
            Target = null;
        }
    }
}