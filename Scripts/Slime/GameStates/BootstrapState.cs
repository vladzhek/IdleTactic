using Cysharp.Threading.Tasks;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using Slime.Data.IDs;
using Slime.Data.Triggers;
using Slime.UI;
using Slime.UI.Common;
using UnityEngine.SceneManagement;
using Logger = Utils.Logger;

namespace Slime.GameStates
{
    public class BootstrapState : State<EGameTriggers>
    {
        private readonly UIManager _uiManager;
        private readonly ISceneModel _sceneModel;

        public BootstrapState(
            UIManager uiManager,
            IGameStateMachine gameStateMachine, 
            ISceneModel sceneModel) : base(gameStateMachine)
        {
            _uiManager = uiManager;
            _sceneModel = sceneModel;
        }

        public override void OnEnter()
        {
            _ = LoadScenes();
        }

        public override void OnExit()
        {
        }

        private async UniTask LoadScenes()
        {
            // NOTE: scene loading service?
            await AddScene(SceneIDs.UI);
            
            _uiManager.Open<LoadingView>();

            await AddScene(SceneIDs.PREFABS);
            await AddScene(SceneIDs.BOOTSTRAP);
            await AddScene(SceneIDs.GAMEPLAY);

            await UniTask.WaitUntil(() => _sceneModel.IsReady);

            StateMachine.FireTrigger(EGameTriggers.LoadProgress);
        }

        private static async UniTask AddScene(string name)
        {
            var scene = SceneManager.GetSceneByName(name);
            var activeName = SceneManager.GetActiveScene().name;
            //Logger.Log($"scene: {name}; active: {activeName}");
            if (name.Equals(activeName))
            {
                Logger.Log($"scene: {name} is active scene!");
                return;
            }
            
            //Logger.Log($"scene: {name}; is loaded: {scene.isLoaded}");
            if (!scene.isLoaded)
            {
                await SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            }
        }
    }
}