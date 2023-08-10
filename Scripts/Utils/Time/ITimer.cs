using System;

namespace Utils.Time
{
    public interface ITimer
    {
        event Action<int> OnTick;
        event Action<ITimer> OnComplete;
        float TimeLeft { get; }
        float Duration { get; }
        Timer SetDuration(float duration);
        Timer Pause();
        Timer Start();
        void Tick(float time);
    }
}