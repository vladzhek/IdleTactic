using System;
using JetBrains.Annotations;

namespace Utils.Time
{
    [UsedImplicitly]
    public class LocalTimeService : ITimeService
    {
        public DateTime CurrentTime => DateTime.Now;

        public float TimeMultiplier { get; set; } = 1;
        public float CooldownMultiplier { get; set; } = 1;
        
        public float DeltaTime => UnityEngine.Time.deltaTime * TimeMultiplier;
        public float CooldownDeltaTime => UnityEngine.Time.deltaTime * TimeMultiplier * CooldownMultiplier;
    }
}