using System.Linq;
using Slime.AbstractLayer.Models;
using Slime.Data;
using Slime.Data.IDs;
using Slime.UI.Common.Tabbar;
using Slime.UI.Store.Shop;
using Slime.UI.Store.Summon;

namespace Slime.UI.Footer.Store
{
    public class StoreTabbarViewModel : TabbarViewModel<TabbarData>
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
                Title = "summon",
                SpriteID = SpritesIDs.SUMMON_ICON,
                ViewType = typeof(SummonView),
            },
            new()
            {
                Title = "package",
                IsUnlocked = false
            },
            new()
            {
                Title = "daily",
                IsUnlocked = false
            },
            new()
            {
                Title = "shop",
                SpriteID = SpritesIDs.SHOP_ICON,
                ViewType = typeof(ShopView),
            },
        };
        
        private StoreTabbarViewModel(
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
            if (!_characterUIModel.IsTabSelected)
            {
                SetActiveIndex(DEFAULT_OPEN_TAB);
            }
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