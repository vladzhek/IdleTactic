using System;
using Services;
using Utils.Time;

namespace Slime.AbstractLayer.Services
{
    public interface IWave : IDisposable
    {
        event Action<IWave> Completed;
        event Action<IWave> Failed;
        event Action<string> UnitDied;
        Timer Timer { get; }
        bool IsActive { get; }
    }
}