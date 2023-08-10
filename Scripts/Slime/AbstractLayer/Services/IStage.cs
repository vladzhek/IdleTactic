using System;
using Slime.AbstractLayer.Configs;
using Slime.Data.Progress;

namespace Slime.AbstractLayer.Services
{
    public interface IStage : IDisposable
    {
        event Action<IStage> Completed;
        event Action<IStage> WaveStarted;
        event Action<IStage> WaveCompleted;
        event Action<IStage> WaveFailed;
        event Action<string> UnitDied;
        bool IsLastWave { get; }
        IStageConfig StageConfig { get; }
        IWave Wave { get; }
        IWaveConfig CurrentWaveConfig { get; }
        StageData StageData { get;}
        bool IsWaveFailed { get; set; }
        void LoadWave(int waveIndex);
        void LoadNextWave();
        void ReloadWave();
    }
}