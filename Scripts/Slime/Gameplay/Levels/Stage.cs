using System;
using System.Collections.Generic;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Configs;
using Slime.AbstractLayer.Services;
using Slime.Data.Progress;
using Slime.Factories;
using Slime.Services.Analytics;
using UnityEngine;
using Utils;
using Logger = Utils.Logger;
using Random = UnityEngine.Random;

namespace Slime.Levels
{
    public class Stage : IStage
    {
        const float OFFSET_FROM_PLAYER = 50f;
        const float SPREAD = 4f;

        public event Action<IStage> Completed;
        public event Action<IStage> WaveStarted;
        public event Action<string> UnitDied;
        public event Action<IStage> WaveCompleted;
        public event Action<IStage> WaveFailed;

        private readonly UnitFactory _unitFactory;
        private IUnit _player;

        private int reloadCount;
        
        public Stage(IStageConfig stageConfig, StageData data, UnitFactory unitFactory, IUnit player)
        {
            _player = player;
            StageConfig = stageConfig;
            _unitFactory = unitFactory;
            StageData = data;
            WaveIndex = data.Wave ?? 0;
        }
        
        public IStageConfig StageConfig { get; }
        public StageData StageData { get; }
        public int StageIndex => StageData.Stage ?? 0;
        public int WaveIndex { get; private set; }
        public bool IsLastWave => WaveIndex + 1 >= StageConfig.Waves.Length;
        public bool IsWaveFailed { get; set; }
        public IWave Wave { get; private set; }
        public IWaveConfig CurrentWaveConfig => StageConfig.Waves[WaveIndex];


        public void LoadNextWave()
        {
            LoadWave(WaveIndex + 1);
        }

        public void LoadWave(int waveIndex)
        {
            WaveIndex = waveIndex % StageConfig.Waves.Length;
            StageData.Wave = WaveIndex;
            Logger.Log($"Load wave {waveIndex} from {StageConfig.Waves.Length} => {WaveIndex}");
            var units = CreateUnits();
            CreateWave(units);
        }

        public void ReloadWave()
        {
            Logger.Log($"Reload wave {StageIndex} {WaveIndex} {reloadCount++}");

            ReleaseWave();

            var units = CreateUnits();
            CreateWave(units);
        }

        public void Dispose()
        {
            ReleaseWave();
        }

        private List<IUnit> CreateUnits()
        {
            var units = new List<IUnit>();
            var avatars = CurrentWaveConfig.UnitsAvatars.GetUnitsAvatars();
            for (var i = 0; i < CurrentWaveConfig.Units.Length; i++)
            {
                CreateUnit(i, avatars, units);
            }

            return units;
        }

        private void CreateUnit(int i, IReadOnlyDictionary<string, IUnitAvatar> avatars, ICollection<IUnit> units)
        {
            var waveUnit = CurrentWaveConfig.Units[i];
            if (!avatars.TryGetValue(waveUnit, out var avatar))
            {
                Logger.Error($"No avatar for {waveUnit}");
                return;
            }

            var unit = _unitFactory.CreateEnemy(waveUnit, avatar, StageIndex, StageConfig);

            unit.ResetParameters();
            var offset = Random.insideUnitSphere * SPREAD;
            offset.z = 0;
            offset = Quaternion.AngleAxis(30, Vector3.right)* offset;
            var position = _player.Position + Vector3.right * OFFSET_FROM_PLAYER + offset;
            var rotation = Quaternion.LookRotation(Vector3.left);
            unit.SetPosition(position, rotation);
            units.Add(unit);
        }

        private void CreateWave(List<IUnit> units)
        {
            Wave = new Wave(units);
            Wave.UnitDied += OnUnitDies;
            Wave.Completed += OnWaveCompleted;
            Wave.Failed += OnWaveFailed;
            
            _player.Die += OnPlayerDied;
            
            IsWaveFailed = false;
            WaveStarted?.Invoke(this);
        }

        private void OnPlayerDied(IUnit player)
        {
            OnWaveFailed(Wave);
        }

        private void OnUnitDies(string unitId)
        {
            UnitDied?.Invoke(unitId);
        }

        private void OnWaveCompleted(IWave obj)
        {
            Logger.Log($"Wave {WaveIndex + 1} of {StageConfig.Waves.Length} completed");

            ReleaseWave();

            WaveCompleted?.Invoke(this);

            if (IsLastWave)
            {
                Logger.Log($"Level {StageIndex} completed");
                Completed?.Invoke(this);
            }
        }

        private void OnWaveFailed(IWave obj)
        {
            Logger.Log($"Wave {WaveIndex} failed");

            ReleaseWave();

            IsWaveFailed = true;
            WaveFailed?.Invoke(this);
        }

        private void ReleaseWave()
        {
            if (Wave == null)
            {
                return;
            }

            Wave.UnitDied -= OnUnitDies;
            Wave.Completed -= OnWaveCompleted;
            Wave.Failed -= OnWaveFailed;
            _player.Die -= OnPlayerDied;

            Wave.Dispose();
        }

        public override string ToString()
        {
            return $"Level {StageIndex + 1} - {WaveIndex + 1} {StageConfig.StageType}";
        }
    }
}