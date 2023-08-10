using Pooling;
using Slime.AbstractLayer;
using UnityEditor;
using UnityEngine;
using Utils.Pooling;
using Logger = Utils.Logger;

namespace Slime.UnityLayer
{
    public class UnitAvatar : PoolObject, IUnitAvatar
    {
        [SerializeField] private Transform _projectileOrigin;
        [SerializeField] private Animator _animator;

        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int MoveBool = Animator.StringToHash("IsMoving");
        private static readonly int DestroyTrigger = Animator.StringToHash("Destroy");
        private static readonly int ResetTrigger = Animator.StringToHash("Reset");

        private string _status;
        private string _health;

        public PoolObject Object => this;
        public Vector3 Position => transform.position;
        public Transform ProjectileOrigin => _projectileOrigin;

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetActiveUnit(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void TriggerAction()
        {
            _animator.SetTrigger(AttackTrigger);
        }

        public void SetMoveState(bool isMoving)
        {
            _animator.SetBool(MoveBool, isMoving);
        }

        public void TriggerDestroy()
        {
            _animator.SetTrigger(DestroyTrigger);
        }

        public void ResetState()
        {
            _animator.SetTrigger(ResetTrigger);
        }

        public void DisplayStatus(string status)
        {
            _status = status;
        }

        public void DisplayHealth(float value)
        {
            _health = $"{value}";
        }

        public void TriggerDamage()
        {
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var position = transform.position;
            Handles.Label(position, _status);
            Handles.Label(position + Vector3.up * 2, _health);
        }
#endif
    }
}