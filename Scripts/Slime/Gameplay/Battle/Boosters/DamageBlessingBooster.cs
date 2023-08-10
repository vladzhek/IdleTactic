using Slime.Data.Boosters;
using Slime.Models;
using Utils.Time;

namespace Slime.Gameplay.Battle.Boosters
{
    public class DamageBlessingBooster : Booster
    {

        public DamageBlessingBooster(BoosterProgressData progress, 
            BoosterData data,
            TimerService timerService) : base(progress, data, timerService)
        {
        }
    }
}