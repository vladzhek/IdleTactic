using System;
using Slime.Data.Enums;
using Slime.Data.Progress;
using Zenject;

namespace Slime.AbstractLayer.Models
{
    public interface IStageModel : IInitializable, IDisposable
    {
        event Action<EStageType> OnChange;
        event Action<bool> OnCycleChange;
        event Action<bool> OnScreenHiddenChange; 
        public StageData Get(EStageType type);
        public EStageType Type { get; }
        bool IsBattleCycled { get; set; }
        bool IsScreenHidden { get; set; }
        public void SetType(EStageType type);
        public void Update(EStageType type, StageData data);
        void SetCycled(bool isCycled);
        void SetScreenHidden(bool isActive);
    }
}