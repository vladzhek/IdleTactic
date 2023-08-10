using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Configs;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Data.Products;

namespace Slime.AbstractLayer.Services
{
    public interface IStageController
    {
        public event Action<IStageController> WaveCompleted;
        public event Action<IStageController> StageChanged;
        public event Action<IStageController> StageCompleted;
        public event Action<IStageController> StageFailed;
        IStage CurrentStage { get; }
        void SetConfigs(IDictionary<EStageType, IStageConfig[]> configs);
        void LoadStageFromScratch();
        void LoadNextLevel();
        event Action<IStageController> StageStarted;
        public EStageType Type { get; }
        List<ResourceData> GetRewardForStage(EStageType stageType);
    }
}