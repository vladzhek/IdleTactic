using Services;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Models.Equipment;
using Slime.AbstractLayer.Services;
using Slime.AbstractLayer.StateMachine;
using Slime.Configs;
using Slime.Data.Enums;
using Slime.Factories;
using Slime.Gameplay;
using Slime.GameStates;
using Slime.Models;
using Slime.Services;
using Slime.Services.Analytics;
using Slime.Services.EffectsService;
using Slime.Services.Purchasing;
using Slime.UI;
using UI.Base;
using UnityEngine;
using Utils.Time;
using Zenject;

namespace Slime.Initialization
{
    public class ServiceInstaller : MonoInstaller
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private AdsService _adsService;
        [SerializeField] private AudioService _audioService;

        public override void InstallBindings()
        {
            BindPrefabs();
            BindFactories();
            BindModels();
            BindServices();
            BindGameEntities();
            BindGameStates();
        }

        private void BindPrefabs()
        {
            Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
            Container.Bind<IAdsService>().FromInstance(_adsService).AsSingle();
            Container.Bind<AudioService>().FromInstance(_audioService).AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IObjectFactory>().To<ZenjectFactory>().AsSingle();
            Container.Bind<UnitFactory>().FromNew().AsSingle();
            Container.Bind<IStageFactory>().To<StageFactory>().AsSingle();
            Container.Bind<ISkillFactory>().To<SkillFactory>().AsSingle();

            Container.BindFactory<ERefreshableEntity, DailyEntityRefresher, DailyEntityRefresher.Factory>();
        }

        private void BindModels()
        {
            Container.Bind<IGameProgressModel>().To<ProgressModel>().AsSingle();
            Container.Bind<ISettingsModel>().To<SettingsModel>().AsSingle();
            Container.Bind<ISpritesModel>().To<SpritesModel>().AsSingle();
            Container.Bind<IResourcesModel>().To<ResourcesModel>().AsSingle();
            
            Container.BindInterfacesTo<CharacterModel>().AsSingle();
            Container.BindInterfacesTo<AttributesModel>().AsSingle();
            Container.BindInterfacesTo<ParametersModel>().AsSingle();
            
            Container.BindInterfacesTo<InventoryModel>().AsSingle();
            Container.BindInterfacesTo<SkillsModel>().AsSingle();
            Container.BindInterfacesTo<CompanionsModel>().AsSingle();

            // NOTE: what is this model?
            Container.Bind<ISceneModel>().To<SceneModel>().AsSingle();
            Container.BindInterfacesTo<StageModel>().AsSingle();
            Container.Bind<IUnitsModel>().To<UnitsModel>().AsSingle();
            Container.BindInterfacesTo<SkillsManager>().AsSingle();
            Container.Bind<IRewardsModel>().To<RewardsModel>().AsSingle();

            Container.BindInterfacesTo<ProductsModel>().AsSingle();
            Container.BindInterfacesTo<SummonModel>().AsSingle();
            Container.BindInterfacesTo<BoostersModel>().AsSingle();
            Container.BindInterfacesTo<OfflineIncomeModel>().AsSingle();
            Container.BindInterfacesTo<TutorialModel>().AsSingle();

            Container.Bind<IFooterUIModel>().To<FooterUIModel>().AsSingle();
            Container.Bind<ICharacterUIModel>().To<CharacterUIModel>().AsSingle();
            Container.Bind<IInventoryUIModel>().To<InventoryUIModel>().AsSingle();
            Container.BindInterfacesTo<DungeonsUIModel>().AsSingle();
            Container.Bind<ISummonInfoUIModel>().To<SummonInfoUIModel>().AsSingle();
            Container.Bind<ISummonResultUIModel>().To<SummonResultUIModel>().AsSingle();
            Container.Bind<IEquipmentItemUIModel>().To<EquipmentItemUIModel>().AsSingle();
            Container.Bind<IRewardsUIModel>().To<RewardsUIModel>().AsSingle();
            Container.Bind<IDialogUIModel>().To<DialogUIModel>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<ITimeService>().To<LocalTimeService>().AsSingle();
            Container.Bind<TimerService>().FromNew().AsSingle();
            Container.BindInterfacesTo<TimeTrackerService>().AsSingle();
            Container.Bind<UpdateLoopsService>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<AimService>().FromNew().AsSingle();
            Container.Bind<ICameraService>().To<CameraService>().AsSingle();
            Container.Bind<IViewsCreator>().To<ViewsCreator>().AsSingle();
            Container.Bind<ISaveService>().To<SaveService>().AsSingle();
            Container.Bind<IEffectsService>().To<EffectsService>().AsSingle();
            Container.BindInterfacesTo<AnalyticsManager>().AsSingle();
            Container.Bind<IAnalyticsItemService>().To<MadPixelsAnalyticsService>().AsSingle();
            Container.BindInterfacesTo<MadPixelsPurchasingService>().AsSingle();
        }

        private void BindGameEntities()
        {
            Container.BindInterfacesTo<Player>().AsSingle();
            Container.Bind<ICompanion>().To<Companion>().AsSingle();
            Container.Bind<IStageController>().To<NormalStageController>().AsSingle();
            Container.Bind<UIManager>().FromNew().AsSingle();
        }

        private void BindGameStates()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();
        }
    }
}