using Slime.Data.Abstract;
using Slime.Data.Enums;

namespace Slime.Data.Summon
{
    public class SummonBaseData : UpgradableData
    {
        public ESummonType Type { get; private set; }

        public override string ID => Type.ToString();
        
        public void SetType(ESummonType type)
        {
            Type = type;
        }
        
        public override string ToString()
        { 
            return $"{base.ToString()};"
                   + $" type: {Type};"
                ;
        }
    }
}