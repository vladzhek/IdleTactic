using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.StateMachine;
using UnityEngine;
using Utils.Time;
using Zenject;

namespace Slime.Initialization
{
    public class Bootstrap : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;
        private IGameProgressModel _progressModel;
        private ITimeService _timeService;

        [Inject]
        private void Construct(IGameStateMachine gameStateMachine, 
            IGameProgressModel progressModel,
            ITimeService timeService)
        {
            _gameStateMachine = gameStateMachine;
            _progressModel = progressModel;
            _timeService = timeService;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            _gameStateMachine?.OnUpdate(_timeService.DeltaTime);
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused)
            {
                _progressModel.Save();
            }
        }
    }
}