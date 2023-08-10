using UnityEngine;
using Utils;
using Zenject;

namespace Slime.Initialization
{
    public class RenderTextureInstaller : MonoInstaller
    {
        [SerializeField] private RenderTextureController _renderTextureController;

        public override void InstallBindings()
        {
            // TODO: wtf is going on?
            Container.Bind<RenderTextureController>()
                .FromComponentInNewPrefab(_renderTextureController).AsSingle();
            
            /*
            switch (PrefabUtility.GetPrefabAssetType(_renderTextureController))
            {
                case PrefabAssetType.NotAPrefab:
                    Container.Bind<RenderTextureController>()
                        .FromInstance(_renderTextureController);
                    break;
                case PrefabAssetType.Regular:
                case PrefabAssetType.Variant:
                    Container.Bind<RenderTextureController>()
                        .FromComponentInNewPrefab(_renderTextureController).AsSingle();
                    break;
                case PrefabAssetType.Model:
                case PrefabAssetType.MissingAsset:
                default:
                    throw new ArgumentOutOfRangeException();
            }
            */
        }
    }
}