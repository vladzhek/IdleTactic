using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs
{
    public abstract class TimerIDs : IDList<TimerIDs>
    {
        public new static string Default => REFRESH;
        
        public const string REFRESH = "refresh";
        public const string REWARD_POPUP_CLOSE = "rewardPopupClose";
    }
}