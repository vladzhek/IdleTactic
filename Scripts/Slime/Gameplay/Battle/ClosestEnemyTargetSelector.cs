using Services;
using Slime.AbstractLayer.Battle;

namespace Slime.Battle
{
    public class ClosestEnemyTargetSelector : ITargetSelector
    {
        private readonly AimService _aimService;

        public ClosestEnemyTargetSelector(AimService aimService)
        {
            _aimService = aimService;
        }

        public IUnit GetTarget()
        {
            return _aimService.GetClosestToPlayerEnemyUnit();
        }
    }
}