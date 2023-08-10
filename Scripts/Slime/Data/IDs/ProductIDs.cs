using System;
using System.Collections.Generic;
using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs
{
    public abstract class ProductIDs : IDList<ProductIDs>
    {
        public new static string Default => throw new NotImplementedException();

        public const string NO_ADS_SKU = "noAds";
        
        public const string HARD_CURRENCY_1 = "hard_currency_1";
        public const string HARD_CURRENCY_2 = "hard_currency_2";
        public const string HARD_CURRENCY_3 = "hard_currency_3";
        public const string HARD_CURRENCY_4 = "hard_currency_4";
        public const string HARD_CURRENCY_5 = "hard_currency_5";
        public const string HARD_CURRENCY_6 = "hard_currency_6";

        public static IEnumerable<string> InApps => new[]
        {
            HARD_CURRENCY_1,
            HARD_CURRENCY_2,
            HARD_CURRENCY_3,
            HARD_CURRENCY_4,
            HARD_CURRENCY_5,
            HARD_CURRENCY_6 
        };
    }
}