using System;
using Slime.Data.Enums;
using Zenject;

namespace Slime.AbstractLayer.Services
{
    public interface ITimeTrackerService: IInitializable, IDisposable
    {
        // time until "refresh" in seconds, ticks each second
        public event Action OnRefreshTimerComplete;
        public event Action<int> OnRefreshTimerTick;
        
        bool IsNewDay();
        
        IEntityRefresher RequestRefresher(ERefreshableEntity summonAds);
    }
}