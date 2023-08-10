using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.AbstractLayer.Stats;
using UnityEngine;
using Attribute = Slime.AbstractLayer.Stats.Attribute;

namespace Slime.AbstractLayer.Battle
{
    public interface IUnit : IDamageable, IDisposable, IUpdatable
    {
        event Action<IUnit> Die;
        event Action<IUnit> PositionChanged;
        event Action<IUnit, float> HealthChanged;
        string ID { get; }
        string Status { get; set; }
        ESide Side { get; }
        IUnitBehaviour Behaviour { get; }
        IUnitAvatar Avatar { get; }
        IUnit Target { get; }
        bool IsActive { get; }
        
        // TODO: refactor companion index
        public int Index { get; set; }

        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 HealthbarPosition { get; }
        void InitializeParameters(Dictionary<string, Attribute> attributes);
        void UpdateParameters(IEnumerable<Attribute> attributes);
        void InjectAvatar(IUnitAvatar avatar);
        void SetBehaviour(IUnitBehaviour unitBehaviour);
        void SetActive(bool isActive);
        void DestroyUnit();
        void SetTarget(IUnit unit);
        void SetPosition(Vector3 position, Quaternion rotation = default);
        void SyncPosition();
        Attribute GetAttribute(string attributeID);
        Parameter GetParameter(string parameterID);
        void ResetParameters();
        void Reset();
        void SetIndex(int index);
    }
}