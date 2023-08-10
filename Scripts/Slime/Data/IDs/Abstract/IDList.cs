using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utils.Extensions;

namespace Slime.Data.IDs.Abstract
{
    public abstract class IDList<T>
    {
        public static string Default => throw new System.NotImplementedException();

        public static IEnumerable<string> Values => typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => 
                fi.IsLiteral && !fi.IsInitOnly || // constant
                !fi.IsLiteral && fi.IsInitOnly // readonly
            ).Select(x => x.GetValue(x).ToString());

        public static IEnumerable<string> ValuesWithPrefix(string prefix) =>
            from value in Values select $"{prefix}{value.Capitalize()}";
    }
}