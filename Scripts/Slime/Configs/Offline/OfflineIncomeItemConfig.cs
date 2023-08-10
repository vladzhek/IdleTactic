using Slime.Configs.Abstract;
using Slime.Data.Offline;
using UnityEngine;

namespace Slime.Configs.Offline
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 11)]
    public class OfflineIncomeItemConfig : ItemConfig<OfflineIncomeData>
    {
        private const string ENTITY = "OfflineIncomeItem";
    }
}