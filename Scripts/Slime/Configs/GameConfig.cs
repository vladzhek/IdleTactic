using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Configs;
using Slime.Configs.Attributes;
using Slime.Configs.Levels;
using Slime.Configs.Rewards;
using Slime.Configs.Units;
using Slime.Data.Enums;
using UnityEngine;

// TODO: remove this
namespace Slime.Configs
{
    public class GameConfig : MonoBehaviour
    {
        [SerializeField] private UnitsAvatars _characters;
        [SerializeField] private UnitsAvatars _companion;
        [SerializeField] private UnitsConfig _units;
        [SerializeField] private LevelsConfig _stages;
        [SerializeField] private BattleRewards _battleRewards;
        [SerializeField] private AttributesUpgradesConfig _attributesUpgrades;
        
        public Dictionary<string, UnitDefaultParameters> UnitAttributes { get; private set; }
        public Dictionary<string, IUnitAvatar> Characters { get; private set; }
        public Dictionary<string, IUnitAvatar> Companions { get; private set; }
        public Dictionary<string, IUpgradeConfig> Attributes { get; private set; }
        public Dictionary<EStageType, RewardsConfig> Rewards { get; private set; }
        public LevelsConfig Stages => _stages;
        
        private void Awake()
        {
            UnitAttributes = _units.UnitsConfigs.ToDictionary(x => x.UnitID, x => x.BaseAttributesValues);
            Characters = _characters.GetUnitsAvatars();
            Companions = _companion.GetUnitsAvatars();
            Attributes =
                _attributesUpgrades.AttributeUpgradesData.ToDictionary(x => x.AttributeID, x => x.Config);
             Rewards = _battleRewards.Config.ToDictionary(x => x.StageType, x => x);
        }
    }
}