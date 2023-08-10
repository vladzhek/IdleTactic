using System;

namespace Utils.Time
{
    public interface ITimeService
    {
        public DateTime CurrentTime { get; }

        public float TimeMultiplier { get; set; }
        public float CooldownMultiplier { get; set; }

        public float DeltaTime { get; }
        public float CooldownDeltaTime { get; }
    }
}