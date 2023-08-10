using Slime.AbstractLayer.Battle;

namespace Slime.Gameplay.Battle.Units
{
    public class PlayerCharacterUnit : UnitBase
    {
        public override ESide Side => ESide.Ally;

        public PlayerCharacterUnit(string id) : base(id)
        {
        }
        
        public override UnityEngine.Vector3 HealthbarPosition => Position + UnityEngine.Vector3.up * 7f;

        protected override void OnDeath()
        {
            Avatar.TriggerDestroy();
            SetTarget(null);
        }
    }
}