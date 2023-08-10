using Utils.Promises;

namespace Slime.AbstractLayer.Battle
{
    public interface IDamageable
    {
        public Promise ApplyDamage(float damage);
    }
}