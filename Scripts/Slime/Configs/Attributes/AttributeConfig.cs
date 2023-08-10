using Slime.Configs.Abstract;
using UnityEngine;

namespace Slime.Configs.Attributes
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class AttributeConfig : ItemConfig<Slime.Data.Attributes.AttributeData>
    {
        private const string ENTITY = "Attribute";
    }
}