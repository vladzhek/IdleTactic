using System;
using UnityEngine;

namespace Utils.Time
{
    public class Timer : ITimer
    {
        public event Action<int> OnTick;  
        public event Action<ITimer> OnComplete;

        private readonly string _id;
        
        private float _duration;
        private float _time;
        
        private float _elapsedSinceLastTick;
        private const float TICK_INTERVAL = 1;

        public bool IsActive { get; private set; }

        public Timer(string id)
        {
            _id = id;
        }
        
        public string ID => _id;

        public float TimeLeft => _duration - _time;
        public float Duration => _duration;

        public Timer SetDuration(float duration)
        {
            _duration = duration;
            return this;
        }

        public Timer Reset()
        {
            _time = 0;
            return this;
        }
        
        public Timer Pause()
        {
            IsActive = false;
            return this;
        }
        
        public Timer Start()
        {
            IsActive = true;
            return this;
        }
        
        public void Tick(float time)
        {
            if (!IsActive)
            {
                return;
            }

            _time += time;
            
            _elapsedSinceLastTick += time;
            if(_elapsedSinceLastTick >= TICK_INTERVAL)
            {
                OnTick?.Invoke(Mathf.CeilToInt(TimeLeft));
                _elapsedSinceLastTick = 0;
            }

            if (_time >= _duration)
            {
                OnComplete?.Invoke(this);
                Reset();
                Pause();
            }
        }
    }
}