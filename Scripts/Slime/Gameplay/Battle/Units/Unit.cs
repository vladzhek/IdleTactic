using System;
using Slime.AbstractLayer.Battle;
using Slime.Gameplay.Battle.Units;

namespace Slime.Battle.Units
{
    [Serializable]
    public class Unit : UnitBase
    {
        public override ESide Side => ESide.Enemy;

        public Unit(string id) : base(id)
        {
        }

        protected override void OnDeath()
        {
        }
    }
}