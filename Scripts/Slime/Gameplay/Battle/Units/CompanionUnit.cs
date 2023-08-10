using System.Dynamic;
using Slime.AbstractLayer.Battle;
using Utils;

namespace Slime.Gameplay.Battle.Units
{
    public class CompanionUnit : UnitBase
    {
        public override ESide Side => ESide.Ally;

        public CompanionUnit(string id) : base(id)
        {
        }

        protected override void OnDeath()
        {
            Avatar.TriggerDestroy();
            SetTarget(null);
        }
    }
}