using System;
using System.Linq;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.UI.Common.Tabbar;

namespace Slime.UI.Footer.Tabbar
{
    // TODO: lock castle
    public class FooterViewModel : TabbarViewModel<FooterLayoutData>
    {
        public event Action<IStageController> OnStageController
        {
            add => _stageController.StageChanged += value;
            remove => _stageController.StageChanged -= value;
        }

        public event Action<ETutorialStage> TutorialChanged
        {
            add => _tutorialModel.OnChange += value;
            remove => _tutorialModel.OnChange -= value;
        }
        
        public ETutorialStage GetCurrentTutorialStage()
        {
            return _tutorialModel.Stage;
        }

        private readonly IFooterUIModel _footerUIModel;
        private readonly IStageController _stageController;
        private readonly ITutorialModel _tutorialModel;

        private FooterViewModel(IFooterUIModel footerUIModel, ISpritesModel spritesModel,
            IStageController stageController, ITutorialModel tutorialModel)
        {
            _footerUIModel = footerUIModel;
            _stageController = stageController;
            _tutorialModel = tutorialModel;

            var icons = new[]
            {
                SpritesIDs.CHARACTER_FOOTER_TAB_ICON,
                SpritesIDs.COMPANIONS_FOOTER_TAB_ICON,
                SpritesIDs.DUNGEONS_FOOTER_TAB_ICON,
                SpritesIDs.CASTLE_FOOTER_TAB_ICON,
                SpritesIDs.STORE_FOOTER_TAB_ICON,
            };

            //Logger.Log($"opened tab: {_footerUIModel.OpenedTab}");

            Data = icons.Select((t, i) =>
                new FooterLayoutData(
                    spritesModel.Get(t), 
                    _footerUIModel.SelectedTab == i)).ToList();
        }

        #region TabbarViewModel implementation

        protected override ITabbarUIModel TabbarUIModel => _footerUIModel;

        #endregion
    }
}