using System.Linq;
using Slime.AbstractLayer.Models;
using Slime.Data;
using Slime.Data.IDs;
using Slime.UI.Character.Equipment;
using Slime.UI.Character.Skills;
using Slime.UI.Common.Tabbar;

namespace Slime.UI.Footer.Character
{
    public class CharacterTabbarViewModel : TabbarViewModel<TabbarData>
    {
        private const bool CAN_TAB_CLOSE = false;
        private const int DEFAULT_OPEN_TAB = 0;
        
        // dependencies
        private readonly UIManager _uiManager;
        private readonly ICharacterUIModel _characterUIModel;
        private readonly ISpritesModel _spritesModel;

        private readonly TabData[] _tabData =
        {
            new()
            {
                Title = "tank",
                SpriteID = SpritesIDs.CHARACTER_ICON,
                ViewType = typeof(EquipmentView)
            },
            new()
            {
                Title = "skills",
                SpriteID = SpritesIDs.SKILL_ICON,
                ViewType = typeof(SkillsView)
            },
            new()
            {
                Title = "relic",
                IsUnlocked = false
            },
            new()
            {
                Title = "mastery",
                IsUnlocked = false
            },
            new()
            {
                Title = "trait",
                IsUnlocked = false
            }
        };

        private CharacterTabbarViewModel(
            UIManager uiManager,
            ICharacterUIModel characterUIModel,
            ISpritesModel spritesModel)
        {
            _uiManager = uiManager;
            _characterUIModel = characterUIModel;
            _spritesModel = spritesModel;
        }

        #region TabbarViewModel implementation

        public override void OnInitialize()
        {
            Data = from item in _tabData
                select new TabbarData(item.IsUnlocked ? _spritesModel.Get(item.SpriteID) : null)
                {
                    Title = item.Title,
                    IsUnlocked = item.IsUnlocked
                };

            CanTabClose = CAN_TAB_CLOSE;
        }

        public override void OnStart()
        {
            var selectedTab = _characterUIModel.SelectedTab;
            SetActiveIndex(selectedTab >= 0 ? selectedTab : DEFAULT_OPEN_TAB);
        }

        protected override ITabbarUIModel TabbarUIModel => _characterUIModel;

        protected override void OnIndexChanged(int index, bool isOpen)
        {
            base.OnIndexChanged(index, isOpen);

            //Logger.Log($"index: {index} isOpen: {isOpen}");

            var data = _tabData[index];
            var type = data.ViewType;

            _uiManager.Open(type);
        }

        #endregion
    }
}