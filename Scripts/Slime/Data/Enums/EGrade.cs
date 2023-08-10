using System;
using System.Collections.Generic;
using System.Linq;
using Slime.Data.Constants;
using UnityEngine;
using Utils.Extensions;

namespace Slime.Data.Enums
{
    public enum EGrade
    {
        F,
        E,
        D,
        C,
        B,
        A,
        S,
        SS
    }
    
    public static class EGradeExtensions {
        public static IEnumerable<EGrade> Values => 
            from EGrade type in Enum.GetValues(typeof(EGrade)) 
            select type;
        
        public static Color GetColor(this EGrade type)
        {
            return type switch
            {
                // TODO: move to colors?
                EGrade.C => ColorExtensions.FromHex("69A197"),
                EGrade.B => ColorExtensions.FromHex("E5F993"),
                EGrade.A => ColorExtensions.FromHex("F9DC5C"),
                EGrade.S => ColorExtensions.FromHex("E9CE2C"),
                EGrade.SS => ColorExtensions.FromHex("BF211E"),
                _ => Colors.LIGHT_GRAY
            };
        }
    }
}