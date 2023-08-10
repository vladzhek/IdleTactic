using Slime.Configs.Abstract;
using UnityEngine;

namespace Slime.Configs.Attributes
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class AttributesConfig : ItemsConfig<AttributeConfig, Slime.Data.Attributes.AttributeData>
    {
        private const string ENTITY = "Attributes";
    }
}