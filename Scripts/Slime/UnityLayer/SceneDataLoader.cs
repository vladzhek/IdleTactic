using Slime.AbstractLayer.Models;
using UnityEngine;
using Zenject;

namespace Slime.UnityLayer
{
    public class SceneDataLoader : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private Transform _playerPoint;
        [SerializeField] private Transform[] _companionsPoints;
        [SerializeField] private Transform[] _wavesPoints;

        private ISceneModel _sceneModel;

        [Inject]
        private void Construct(ISceneModel sceneModel)
        {
            _sceneModel = sceneModel;
        }

        private void Awake()
        {
            _sceneModel.SetRoot(_root);
            _sceneModel.SetPlayerPosition(_playerPoint);
            _sceneModel.SetCompanionsPositions(_companionsPoints);
            _sceneModel.SetWavesPositions(_wavesPoints);
            _sceneModel.SetReady(true);
        }
    }
}