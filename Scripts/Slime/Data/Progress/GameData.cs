using System;
using System.Collections.Generic;
using Slime.Data.Abstract;
using Slime.Data.Boosters;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Data.Products;
using Slime.Data.Progress.Abstract;

namespace Slime.Data.Progress
{
    [Serializable]
    public class GameData : GenericData
    {
        // TODO: add unity version
        public string UserID => ID;
        // ReSharper disable once InconsistentNaming
        public string EquippedCharacterID;

        public Dictionary<string, ProgressData> CharacterData;
        public Dictionary<string, ProgressData> AttributesData;
        public Dictionary<string, ProgressData> InventoryData;
        public Dictionary<EInventoryType, string> EquippedInventoryData;
        public Dictionary<string, ProgressData> SkillsData;
        public Dictionary<int, string> EquippedSkillsData;
        public Dictionary<string, ProgressData> CompanionsData;
        public Dictionary<int, string> EquippedCompanionsData;
        
        public Dictionary<EStageType, StageData> StageData;
        public Dictionary<EResource, ResourceData> ResourcesData;
        public Dictionary<string, ProgressData> SummonData;
        public Dictionary<string, BoosterProgressData> BoosterData;
        public Dictionary<string, string> SettingsData;

        // TODO: this should not be tied to resource, because you can watch ads for lots of other things
        public Dictionary<EResource, int> AdsData;
        public DateTime DateOfLastSession = DateTime.Now;
        
        public static GameData Default() => new()
        {
            ID = "CPI",
        };

        public GameData()
        {
            
        }
        
        public GameData(string id)
        {
            ID = id;
            EquippedCharacterID = CharacterIDs.Default;

            // TODO: move to ResourceModel
            //AddedDungeonKeysOnFirstPlay();
        }

        public override string ToString()
        {
            return $"{base.ToString()} id {ID}; character: {EquippedCharacterID}";
        }
        
        /*
        private void AddedDungeonKeysOnFirstPlay()
        {
            var bossRushResource = new ResourceData(ResourceIDs.BOSS_RUSH)
            {
                Quantity = 2
            };

            var goldRushResource = new ResourceData(ResourceIDs.GOLD_RUSH)
            {
                Quantity = 2
            };

            ResourcesData.Add(ResourceIDs.BOSS_RUSH, bossRushResource);
            ResourcesData.Add(ResourceIDs.GOLD_RUSH, goldRushResource);
        }
        */
    }
}