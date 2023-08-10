using System.Collections.Generic;
using JetBrains.Annotations;
using Services;
using Utils.UpdateLoops;

namespace Utils.Time
{
    [UsedImplicitly]
    public class TimerService : IUpdateInLoop
    {
        // TODO: make timer execute once a second
        //private const int SECOND = 1;

        private UpdateLoopsService _updateLoopsService;

        private readonly Dictionary<string, ITimer> _timers = new();

        public EUpdateLoop UpdateLoop => EUpdateLoop.Update;
        public bool IsActive => _timers.Count > 0;

        public TimerService(UpdateLoopsService updateLoopsService)
        {
            updateLoopsService.RegisterForUpdate(this);
        }

        public Dictionary<string, ITimer> Timers => _timers;

        public Timer CreateTimer(string id, int duration)
        {
            // clean old timer
            RemoveTimer(id);
            
            // creating new timer    
            var timer = new Timer(id).SetDuration(duration).Reset();
            _timers[id] = timer;

            timer.OnComplete += OnTimerCompleted;
            timer.Start();
            return timer;
        }

        public void RemoveTimer(string id)
        {
            var timer = _timers.GetValueOrDefault(id);
            if (timer != null)
            {
                timer.Pause();
                timer.OnComplete -= OnTimerCompleted;
                _timers[id] = null;
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var timer in _timers)
            {
                timer.Value.Tick(deltaTime);
            }
        }

        private static void OnTimerCompleted(ITimer timer)
        {
            timer.Pause();
            timer.OnComplete -= OnTimerCompleted;
        }
    }
}