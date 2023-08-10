using System.Collections.Generic;
using Slime.AbstractLayer.Stats;

namespace Slime.AbstractLayer.Battle
{
    public interface IAttack
    {
        public IUnit Target { get; }
        public float Damage { get; }

        public List<Modifier> Modifiers { get; }
    }
}