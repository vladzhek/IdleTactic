using System;

namespace Slime.Data.Progress.Abstract
{
    [Serializable]
    public class ProgressData
    {
        public int? Level;
        public float? Quantity;

        public override string ToString()
        {
            return $"{base.ToString()} level: {Level} quantity: {Quantity}";
        }
    }
}