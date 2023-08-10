using UnityEngine;

namespace Utils.Extensions
{
    public static class NumberExtensions
    {
        private const string FORMAT = "#,0.###;#,0.-#";
        private const string SHORTER_FORMAT = "#,0.##;#,0.-#";
        private const string SHORTEST_FORMAT = "#,0.#;#,0.-#";
        
        public static string ToMetricPrefixString(this int value)
        {
            return ((float)value).ToMetricPrefixString();
        }

        public static string ToMetricPrefixString(this float value)
        {
            value = Mathf.Abs(value);

            var suffixIndex = (int)Mathf.Floor(Mathf.Log10(value) / 3f);
            if (suffixIndex < 0) suffixIndex = 0;
            
            var number = value / Mathf.Pow(1000f, suffixIndex);
            var suffix = suffixIndex < NumberSuffixes.Suffixes.Length 
                ? NumberSuffixes.Suffixes[suffixIndex] 
                : "ALOT";
            
            return number.ToString(number switch
            {
                > 100 => SHORTEST_FORMAT,
                > 10 => SHORTER_FORMAT,
                _ => FORMAT
            }) + suffix;
        }

        private static class NumberSuffixes
        {
            private const string EMPTY = "";
            private const string ONE_THOUSAND = "K";
            private const string ONE_MILLION = "M";
            private const string ONE_BILLION = "B";
            private const string ONE_TRILLION = "T";
            private const string ONE_QUADRILLION = "Q";
            private const string ONE_QUINTILLION = "Qi";
            private const string ONE_SEXTILLION = "Sx";
            private const string ONE_SEPTILLION = "Sp";
            private const string ONE_OCTILLION = "O";
            private const string ONE_NONILLION = "N";
            private const string ONE_DECILLION = "D";

            public static readonly string[] Suffixes = {
                EMPTY,
                ONE_THOUSAND,
                ONE_MILLION,
                ONE_BILLION,
                ONE_TRILLION,
                ONE_QUADRILLION,
                ONE_QUINTILLION,
                ONE_SEXTILLION,
                ONE_SEPTILLION,
                ONE_OCTILLION,
                ONE_NONILLION,
                ONE_DECILLION,
            };
        }
    }
}