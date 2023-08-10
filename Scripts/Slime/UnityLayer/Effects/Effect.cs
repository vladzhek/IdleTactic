using DG.Tweening;
using Pooling;
using Slime.AbstractLayer;
using UnityEngine;
using Utils.Pooling;
using Utils.Promises;

namespace Slime.Services.EffectsService
{
    public class Effect : PoolObject, IEffect
    {
        [SerializeField] private ParticleSystem _creationEffect;
        [SerializeField] private ParticleSystem _baseEffect;
        [SerializeField] private ParticleSystem _endingEffect;
        
        private Promise _promise;
        private float _delay;
        private float _lifetime;
        private float _timeActive;
        private int _stage;
        
        public override void OnExtractFromPool()
        {
            base.OnExtractFromPool();

            Transform.DOKill();

            _stage = 0;
            _timeActive = 0;
        }

        public void Update()
        {
            switch (_stage)
            {
                case 0:
                    PlayInitialEffect();
                    _stage++;
                    break;
                case 1:
                    PlayBaseEffect();
                    _stage++;
                    break;
                case 2:
                    _timeActive += Time.deltaTime;
                    if (_timeActive > _delay)
                    {
                        _timeActive = 0;
                        _stage++;
                    }

                    break;
                case 3:
                    PlayEndingEffect();
                    _promise?.Execute();
                    _promise = null;
                    _stage++;
                    break;
                case 4:
                    _timeActive += Time.deltaTime;
                    if (_timeActive > _lifetime)
                    {
                        Release();
                    }

                    break;
            }
        }

        public IEffect SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }

        public IEffect SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
            return this;
        }

        public IEffect SetParent(Transform parent)
        {
            transform.SetParent(parent);
            return this;
        }

        public IEffect SetDelay(float delay)
        {
            _delay = delay;
            return this;
        }

        public IEffect SetLifetime(float lifetime)
        {
            _lifetime = lifetime;
            return this;
        }

        public IEffect SetPromise(Promise promise)
        {
            _promise = promise;
            return this;
        }

        private void PlayInitialEffect()
        {
            if (_creationEffect != null)
            {
                _creationEffect.Play();
            }
        }

        private void PlayBaseEffect()
        {
            if (_baseEffect != null)
            {
                _baseEffect.Play();
            }
        }

        private void PlayEndingEffect()
        {
            if (_endingEffect != null)
            {
                _endingEffect.Play();
            }
        }
    }
}