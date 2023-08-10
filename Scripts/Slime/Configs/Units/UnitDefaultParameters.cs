using Slime.Configs.Attributes;
using UnityEngine;

namespace Slime.Configs
{
    [CreateAssetMenu(fileName = "UnitParameters", menuName = "Assets/Units/Parameters", order = 0)]
    public class UnitDefaultParameters : ScriptableObject
    {
        [SerializeField] private AttributeData[] _attributes;

        public AttributeData[] Attributes => _attributes;
    }
}