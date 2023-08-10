using Slime.Configs.Abstract;
using Slime.Data.Inventory;
using UnityEngine;

namespace Slime.Configs.Companions
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class CompanionConfig : ItemConfig<CompanionData>
    {
        private const string ENTITY = "Companion";
    }
}