using System;
using Slime.Data.Enums;

namespace Slime.Data.Progress
{
    [Serializable]
    public class StageData
    {
        public EStageType? Type;
        public int? Stage;
        public int? Wave;
        public bool? IsCycled;
        public int? Count;

        public static StageData Default() => new()
        {
            Type = EStageType.Default,
            Stage = 0,
            Wave = 0,
            IsCycled = false
        };

        public StageData()
        {
            
        }
        
        public StageData(EStageType type)
        {
            Type = type;
            Stage = 0;
            Wave = 0;
            Count = 0;
            IsCycled = false;
        }

        public override string ToString()
        {
            return $"{base.ToString()} type: {Type}; stage {Stage}; wave {Wave}; cycled {IsCycled}";
        }
    }
}