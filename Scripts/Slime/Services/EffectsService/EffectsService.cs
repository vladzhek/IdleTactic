using System.Collections.Generic;
using Pooling;
using Slime.AbstractLayer;
using Slime.Configs.Effects;
using Slime.Data.IDs;
using UnityEngine;
using Utils;
using Utils.Pooling;
using Logger = Utils.Logger;

namespace Slime.Services.EffectsService
{
    public interface IEffectsService
    {
        IEffect RequestEffect(EffectRequest request);
    }

    public class EffectsService : IEffectsService
    {
        private readonly Dictionary<string, IPool> _effectPools = new();
        private readonly Dictionary<string, IEffect> _effects = new();
        private readonly AudioService _audioService;

        public EffectsService(AudioService audioService)
        {
            _audioService = audioService;

            var effectsHolder = Resources.Load<EffectsHolder>(EffectsHolder.Path);
            foreach (var entry in effectsHolder.EffectEntries)
            {
                _effects[entry.ID] = entry.Effect;
            }
        }

        public IEffect RequestEffect(EffectRequest request)
        {
            if (!_effectPools.TryGetValue(request.ID, out var pool))
            {
                if (!_effects.TryGetValue(request.ID, out var effectPrefab))
                {
                    Logger.Error($"no effect for id {request.ID}");
                }

                if (effectPrefab == null)
                {
                    Logger.Error($"effect for id {request.ID} is null");
                }

                pool = new UnityPool(
                    () => Object.Instantiate(effectPrefab!.Transform.gameObject).GetComponent<PoolObject>(), 2);

                _effectPools.Add(request.ID, pool);
            }

            var effect = pool.GetObject<IEffect>()
                .SetParent(request.Parent)
                .SetPosition(request.Position)
                .SetRotation(request.Rotation);

            effect.SetLifetime(request.Lifetime);
            effect.SetDelay(request.Delay);
            
            effect.Transform.DOMoveToTarget(request.Target, request.Delay);
            _audioService.Play(AudioIDs.SHOOT);

            return effect;
        }
    }
}