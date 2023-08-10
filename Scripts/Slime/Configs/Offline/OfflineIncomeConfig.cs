using Slime.Configs.Abstract;
using Slime.Data.Offline;
using UnityEngine;

namespace Slime.Configs.Offline
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class OfflineIncomeConfig : ItemsConfig<OfflineIncomeItemConfig, OfflineIncomeData>
    {
        public const string PATH = Data.Constants.System.CONFIG_PATH + ENTITY;
        private const string ENTITY = "OfflineIncome";
    }
}