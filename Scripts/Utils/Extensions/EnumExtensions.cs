using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Extensions
{
    public static class EnumExtensions<TEnum> where TEnum : struct
    {
        public static IEnumerable<TEnum> Values => 
            from TEnum item in Enum.GetValues(typeof(TEnum)) 
            select item;
    }
}