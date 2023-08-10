using Slime.AbstractLayer.Battle;

namespace Slime.AbstractLayer
{
    public interface IPlayer
    {
        public IUnit Unit { get; }
        void SetUnit(IUnit unit);
    }
}