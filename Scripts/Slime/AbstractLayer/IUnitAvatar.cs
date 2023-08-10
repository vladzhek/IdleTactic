using Pooling;
using UnityEngine;
using Utils.Pooling;

namespace Slime.AbstractLayer
{
    public interface IUnitAvatar : IPoolObject
    {
        public PoolObject Object { get; }
        public Transform ProjectileOrigin { get; }

        public void SetPosition(Vector3 position);
        public void SetActiveUnit(bool isActive);
        public void SetRotation(Quaternion rotation);
        public void TriggerAction();
        public void SetMoveState(bool isMoving);
        public void TriggerDestroy();
        public void ResetState();
        public void DisplayStatus(string status);
        public void DisplayHealth(float value);
        public void TriggerDamage();
    }
}