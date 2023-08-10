using UnityEngine;

namespace Utils.Extensions
{
    public class ColorExtensions
    {
        public static Color FromHex(string hexString)
        {
            if (!hexString.StartsWith("#"))
            {
                hexString = $"#{hexString}";
            }
            
            ColorUtility.TryParseHtmlString(hexString, out var color);
            return color;
        }
    }
}