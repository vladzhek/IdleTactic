using UnityEngine;

namespace Utils
{
    public static class Progression
    {
        public static float Linear(float baseValue, int level, float step = 1)
        {
            return baseValue + level * step;
        }

        public static float Geometric(float baseValue, int level, float step = 2)
        {
            return baseValue + Mathf.Pow(level, step);
        }

        public static float Exponential(float baseValue, int level, float step = 2)
        {
            return baseValue + Mathf.Pow(step, level);
        }
        
        public static float Percentage(float baseValue, int level, float step = .1f)
        {
            return baseValue * Mathf.Pow(1 + step, level);
        }
    }
}