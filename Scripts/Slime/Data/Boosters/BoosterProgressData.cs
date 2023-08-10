namespace Slime.Data.Boosters
{
    public class BoosterProgressData
    {
        public string ID;
        public int Level;
        public int Amount;
        public float BoosterEffectValue;
        public int TimeToDeactivation;
        public bool IsActive;

        public BoosterProgressData(string id, float boosterEffectValue)
        {
            ID = id;
            BoosterEffectValue = boosterEffectValue;
        }
    }
}