using Slime.AbstractLayer.StateMachine;
using Slime.Data.IDs;

namespace Slime.Gameplay.Behaviours.States.EnemyStates
{
    public class AttackState : UnitBehaviourState
    {
        private float _timeSinceAttack;

        public AttackState(IUnitBehaviour unitBehaviour) : base(unitBehaviour)
        {
        }

        public override void OnUpdate(float delta)
        {
            if (UnitBehaviour.Unit.Target == null)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.FindTarget);
                return;
            }

            var targetHealth = UnitBehaviour.Unit.Target.GetParameter(AttributeIDs.Health);
            if (UnitBehaviour.Unit.Target == null || targetHealth.PromisedValue <= 0)
            {
                UnitBehaviour.SetState(EBehaviourTrigger.FindTarget);
                return;
            }

            _timeSinceAttack += delta;

            if (_timeSinceAttack > 1 / UnitBehaviour.Unit.GetAttribute(AttributeIDs.AttackSpeed).FinalValue)
            {
                Attack();
            }
        }

        private void Attack()
        {
            var damage = UnitBehaviour.Unit.GetAttribute(AttributeIDs.Damage).FinalValue;

            UnitBehaviour.Unit.Target.ApplyDamage(damage).Execute();
            UnitBehaviour.Unit.Avatar.TriggerAction();
            _timeSinceAttack = 0;
        }
    }
}