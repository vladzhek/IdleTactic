using Slime.AbstractLayer.Battle;

namespace Slime.AbstractLayer.Models
{
    // NOTE: why is this not in Data?
    public struct HealthValue
    {
        public IUnit Unit;
        public float Current;
        public float Max;
    }
}