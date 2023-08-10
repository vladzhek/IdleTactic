using Slime.Configs.Abstract;
using Slime.Data.Inventory;
using UnityEngine;

namespace Slime.Configs.Companions
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class CompanionsConfig : ItemsConfig<CompanionConfig, CompanionData>
    {
        private const string ENTITY = "Companions";
    }
}