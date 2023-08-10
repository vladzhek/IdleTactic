using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs
{
    public abstract class CharacterIDs : IDList<CharacterIDs>
    {
        public new static string Default => TANK_1;
        
        public const string TANK_1 = "characterTank1";
        public const string TANK_2 = "characterTank2";
        public const string TANK_3 = "characterTank3";
        public const string TANK_4 = "characterTank4";
        public const string TANK_5 = "characterTank5";
    }
}