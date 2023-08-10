using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs
{
    public class AdPlacementIDs : IDList<AdPlacementIDs>
    {
        // NOTE: what for?
        //[ListDrawerSettings(ShowIndexLabels = true)]
        //public List<string> PlacementIDs = new() { BOOSTER, SUMMON, DUNGEON, OFFLINE_INCOME };

        public const string BOOSTER = "booster";
        public const string SUMMON = "summon";
        public const string DUNGEON = "dungeon";
        public const string OFFLINE_INCOME = "offlineIncome";
    }
}