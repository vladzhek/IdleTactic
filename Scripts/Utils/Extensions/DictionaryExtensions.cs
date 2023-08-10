using System.Collections.Generic;

namespace Utils.Extensions
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, int> Increment<TKey>(
            this Dictionary<TKey, int> input,
            TKey key,
            int value)
        {
            input.TryAdd(key, default);
            input[key] += value;
            return input;
        }
        
        public static Dictionary<TKey, float> Increment<TKey>(
            this Dictionary<TKey, float> input,
            TKey key,
            float value)
        {
            input.TryAdd(key, default);
            input[key] += value;
            return input;
        }
    }
}