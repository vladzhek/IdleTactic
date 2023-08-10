using System.Collections.Generic;
using System.Linq;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.Services;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;
using Utils;
using Utils.Pooling;
using Utils.Time;
using Zenject;

namespace Slime.UI.Health
{
    public class HealthView : View<HealthViewModel>, IUpdatable
    {
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private DamageText _damageText;

        private readonly Dictionary<IUnit, HealthBar> _healthBars = new();
        private readonly List<DamageText> _damageTexts = new();
        private UnityPool _healthBarPool;
        private UnityPool _damageTextPool;
        private ICameraService _cameraService;
        private ITimeService _timeService;

        public override UILayer Layer => UILayer.Background;

        [Inject]
        protected void Construct(ICameraService cameraService, ITimeService timeService)
        {
            _timeService = timeService;
            _cameraService = cameraService;
        }

        protected override void Awake()
        {
            base.Awake();
            
            // NOTE: view can create views?
            _healthBarPool = new UnityPool(() => Factory.CreateObject(_healthBar, transform));
            _damageTextPool = new UnityPool(() => Factory.CreateObject(_damageText, transform));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            ViewModel.DamageDone += OnDamageDone;
            ViewModel.UnitsHealth.ItemChanged += OnHealthChanged;
            ViewModel.UnitsHealth.ItemRemoved += OnUnitRemoved;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            ViewModel.DamageDone -= OnDamageDone;
            ViewModel.UnitsHealth.ItemChanged -= OnHealthChanged;
            ViewModel.UnitsHealth.ItemRemoved -= OnUnitRemoved;
        }

        private void Update()
        {
            if (ViewModel.UnitsHealth.Dictionary.Count > 0)
            {
                OnUpdate(0);
            }

            foreach (var damageText in _damageTexts.ToList())
            {
                damageText.OnUpdate(_timeService.DeltaTime);
            }
        }

        private void OnDamageDone(Vector3 position, float value)
        {
            var uiPosition = position.BetweenCameraSpace(_cameraService.MainCamera, _cameraService.UiCamera);
            var damageText = _damageTextPool.GetObject<DamageText>(uiPosition);
            damageText.SetValue(uiPosition, value);
            damageText.ReturnToPool += OnDamageTextReturnedToPool;

            _damageTexts.Add(damageText);
        }

        private void OnDamageTextReturnedToPool(IPoolObject poolObject)
        {
            _damageTexts.Remove(poolObject as DamageText);
            poolObject.ReturnToPool -= OnDamageTextReturnedToPool;
        }

        private void OnHealthChanged(IUnit unit, HealthValue healthValue)
        {
            if (!_healthBars.TryGetValue(unit, out var healthBar))
            {
                var position = unit.Position.BetweenCameraSpace(_cameraService.MainCamera, _cameraService.UiCamera);
                healthBar = _healthBarPool.GetObject<HealthBar>(position);
                _healthBars.Add(unit, healthBar);
            }

            healthBar.SetValue(healthValue.Current / healthValue.Max);
        }

        private void OnUnitRemoved(IUnit unit, HealthValue healthValue)
        {
            if (!_healthBars.TryGetValue(unit, out var healthBar))
            {
                return;
            }

            _healthBars.Remove(unit);
            healthBar.Release();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var (unit, healthBar) in _healthBars)
            {
                var offsetPosition = unit.HealthbarPosition;
                healthBar.Transform.position = offsetPosition.BetweenCameraSpace(_cameraService.MainCamera, _cameraService.UiCamera);
            }
        }
    }
}