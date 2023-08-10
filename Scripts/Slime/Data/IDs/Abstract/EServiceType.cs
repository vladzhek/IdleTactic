using System;

namespace Slime.Services.Analytics
{
    public enum EEventType
    {
        StageStart,
        StageFinish,
        StageDie,
    }
    
    public static class EventTypeExtensions 
    {
        public static string ToMadPixelsEventID(this EEventType type)
        {
            return type switch
            {
                EEventType.StageStart => "level_start",
                EEventType.StageFinish => "level_finish",
                _ => type.ToString()
            };
        }
    }
}