using Slime.AbstractLayer.Configs;
using UnityEngine;

// TODO: remove
namespace Slime.Configs.Attributes
{
    [CreateAssetMenu(fileName = "AttributesUpgradesConfig", menuName = "Assets/Upgrades/Config", order = 0)]
    public class AttributesUpgradesConfig : ScriptableObject
    {
        [SerializeField] private AttributeUpgradesData[] _attributeUpgradesData;

        public IAttributeUpgradesData[] AttributeUpgradesData => _attributeUpgradesData;
    }
}