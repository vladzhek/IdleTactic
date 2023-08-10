using System;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Utils.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var chars = input.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
        
        public static string Uncapitalize(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var chars = input.ToCharArray();
            chars[0] = char.ToLower(chars[0]);
            return new string(chars);
        }
        
        public static string Resolve(this string input, params object[] data)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var i = 0;
            foreach (var item in data)
            {
                input = input.Replace($"{{{i}}}", item.ToString());    
                i++;
            }
            
            return input;
        }
        
        public static string Resolve(this string input, Color color, params object[] data)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var i = 0;
            foreach (var item in data)
            {
                var from = $"{{{i}}}";
                var to = $"{item}";
                //Logger.Log($"input: {input}; from: {from}; to: {to}");

                input = input.Replace(from, $"<#{ColorUtility.ToHtmlStringRGB(color)}>{to}</color>");    
                i++;
            }
            
            return input;
        }

        public static string TrimEndDigits(this string input)
        {
            return new string(input.TrimEnd(input.Where(char.IsDigit).ToArray()));
        }

        public static string CurrencyCodeToSymbol(this string input)
        {
            try
            {
                //var culture = new CultureInfo(input);
                //var region = new RegionInfo(culture.LCID);
                var region = new RegionInfo(input);

                return region.CurrencySymbol;
            }
            catch (Exception e)
            {
                Logger.Warning(e.Message);
            }

            return input;
        }
    }
}