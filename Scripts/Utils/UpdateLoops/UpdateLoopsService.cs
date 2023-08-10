using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Utils.Time;
using Utils.UpdateLoops;
using Zenject;

namespace Services
{
    public class UpdateLoopsService : MonoBehaviour
    {
        private ITimeService _timeService;
        private readonly Dictionary<EUpdateLoop, List<IUpdateInLoop>> _updatables = new();
        private readonly List<IUpdateInLoop> _deadUpdatables = new();

        [Inject]
        public void Construct(ITimeService timeService)
        {
            _timeService = timeService;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            UpdateLoop(EUpdateLoop.PreUpdate);
            UpdateLoop(EUpdateLoop.Update);
            UpdateLoop(EUpdateLoop.Skills);
            UpdateLoop(EUpdateLoop.PostUpdate);
        }

        private void LateUpdate()
        {
            UpdateLoop(EUpdateLoop.LateUpdate);
        }

        private void FixedUpdate()
        {
            UpdateLoop(EUpdateLoop.FixedUpdate);
        }

        public void RegisterForUpdate(IUpdateInLoop updateInLoop)
        {
            if (!_updatables.TryGetValue(updateInLoop.UpdateLoop, out var updatables))
            {
                updatables = new List<IUpdateInLoop>();
                _updatables[updateInLoop.UpdateLoop] = updatables;
            }

            updatables.Add(updateInLoop);
        }

        public void RemoveFromUpdate(IUpdateInLoop updateInLoop)
        {
            if (!_updatables.TryGetValue(updateInLoop.UpdateLoop, out var updatables))
            {
                return;
            }

            if (updatables.Contains(updateInLoop))
            {
                updatables.Remove(updateInLoop);
            }
        }

        private void UpdateLoop(EUpdateLoop updateLoop)
        {
            if (!_updatables.TryGetValue(updateLoop, out var updatables))
            {
                return;
            }

            foreach (var updatable in updatables.ToList())
            {
                if (updatable == null)
                {
                    _deadUpdatables.Add(updatable);
                    continue;
                }

                if (!updatable.IsActive)
                {
                    continue;
                }

                updatable.Update(GetDeltaTime(updateLoop));
            }

            foreach (var updatable in _deadUpdatables)
            {
                updatables.Remove(updatable);
            }

            _deadUpdatables.Clear();
        }

        private float GetDeltaTime(EUpdateLoop loop)
        {
            return loop switch
            {
                EUpdateLoop.FixedUpdate => Time.fixedDeltaTime,
                EUpdateLoop.Skills => _timeService.CooldownDeltaTime,
                _ => _timeService.DeltaTime,
            };
        }
    }
}