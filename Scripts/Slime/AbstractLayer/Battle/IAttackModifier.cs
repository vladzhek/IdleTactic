namespace Slime.AbstractLayer.Battle
{
    public interface IAttackModifier
    {
        public IAttack ModifyAttack(IAttack attack);
    }
}