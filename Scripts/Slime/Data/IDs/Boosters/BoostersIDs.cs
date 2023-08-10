using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs.Boosters
{
    public abstract class BoostersIDs : IDList<BoostersIDs>
    {
        public const string INCOME = "boosterIncome";
        public const string ATK = "boosterATK";
        public const string COOLDOWN = "boosterCooldown";
    }
}