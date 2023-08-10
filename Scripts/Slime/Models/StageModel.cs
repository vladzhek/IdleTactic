using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Slime.Data.Progress;
using Slime.Models.Abstract;
using Utils.Extensions;

namespace Slime.Models
{
    [UsedImplicitly]
    public class StageModel : BaseProgressModel, IStageModel
    {
        public event Action<EStageType> OnChange;
        public event Action<bool> OnCycleChange;
        public event Action<bool> OnScreenHiddenChange;
        
        public bool IsBattleCycled { get; set; }
        public bool IsScreenHidden { get; set; }

        public EStageType Type { get; private set; } = EStageType.Default;

        private StageModel(IGameProgressModel progressModel) : base(progressModel)
        {
        }

        protected override void OnProgressLoaded()
        {
            base.OnProgressLoaded();
            
            IsBattleCycled = Get(EStageType.Default).IsCycled ?? false;
        }

        #region IStageModel implementation

        public StageData Get(EStageType type)
        {
            var stagesData = GameData.StageData;
            if (stagesData == null)
            {
                stagesData = new Dictionary<EStageType, StageData>();
                GameData.StageData = stagesData;
            }

            var stageData = stagesData.GetValueOrDefault(type);
            if (stageData == null)
            {
                stageData = new StageData(type);
                stagesData.Add(type, stageData);
            }
            
            return stageData;
        }

        public void Update(EStageType type, StageData data)
        {
            var currentStageData = Get(type);
            data.CopyFieldsTo(currentStageData);
        }

        #endregion

        public void SetType(EStageType type)
        {
            Type = type;
            OnChange?.Invoke(type);
        }

        public void SetScreenHidden(bool isActive)
        {
            IsScreenHidden = isActive;
            OnScreenHiddenChange?.Invoke(isActive);
        }

        public void SetCycled(bool isCycled)
        {
            if (Type == EStageType.Default)
            {
                IsBattleCycled = isCycled;
                
                var stageData = Get(EStageType.Default);
                stageData.IsCycled = isCycled;

                if (stageData.Wave > 0)
                {
                    stageData.Wave -= 1;
                }
                else if (stageData.Stage > 0)
                {
                    stageData.Stage -= 1;
                }

                Update(EStageType.Default, stageData);
            }

            OnCycleChange?.Invoke(isCycled);
        }
    }
}