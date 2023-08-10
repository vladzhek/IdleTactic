using Slime.AbstractLayer.Models;
using Slime.Data.Boosters;
using Slime.Models;
using Utils;
using Utils.Time;

namespace Slime.Gameplay.Battle.Boosters
{
    public class GoldBlessingBooster : Booster
    {
        public GoldBlessingBooster(BoosterProgressData progress, BoosterData data,
            TimerService timerService) : base(progress, data, timerService)
        {
        }
    }
}