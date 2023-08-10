using Slime.AbstractLayer.Services;
using UnityEngine;
using Zenject;

namespace Slime.UnityLayer
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;

        private IStageController _stageController;

        [Inject]
        private void Construct(IStageController stageController)
        {
            _stageController = stageController;
            _stageController.StageChanged += OnStageProceed;
        }

        private void OnDestroy()
        {
            _stageController.StageChanged -= OnStageProceed;
        }

        private void OnStageProceed(IStageController stageFactory)
        {
            _background.sprite = _stageController.CurrentStage.StageConfig.EnvironmentData.Background;
        }
    }
}