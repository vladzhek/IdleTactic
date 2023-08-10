using System;
using Slime.Data.Boosters;
using Utils.Time;

namespace Slime.AbstractLayer.Models
{
    // NOTE: this is not model
    public interface IBooster
    {
        event Action<string> OnDeactivate;
        void Activate();
        void Deactivate();
        void Upgrade();
        BoosterProgressData Progress { get; }
        BoosterData Data { get; }
        Timer Timer { get; }

        float BlessingValue { get; }
    }
}