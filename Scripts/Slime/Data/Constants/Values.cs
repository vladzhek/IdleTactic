using System.Diagnostics.CodeAnalysis;

namespace Slime.Data.Constants
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Values
    {
        public const float SOFT_CURRENCY_INITIAL_QUANTITY = 50;
        public const float HARD_CURRENCY_INITIAL_QUANTITY = 500;
        public const float BOSS_RUSH_CURRENCY_INITIAL_QUANTITY = 2;
        public const float GOLD_RUSH_CURRENCY_INITIAL_QUANTITY = 2;
        public const float CHARACTER_UPGRADE_CURRENCY_INITIAL_QUANTITY = 5;
        
        // footer tabs
        public const int FOOTER_TAB_CHARACTER = 0;
        public const int FOOTER_TAB_COMPANIONS = 1;
        public const int FOOTER_TAB_DUNGEONS = 2;
        public const int FOOTER_TAB_CASTLE = 3;
        public const int FOOTER_TAB_STORE = 4;
        
        // store tabs
        public const int STORE_TAB_SUMMON = 0;
        public const int STORE_TAB_SHOP = 3;
        
        public const int REWARD_POPUP_CLOSE_DELAY = 3;
    }
}