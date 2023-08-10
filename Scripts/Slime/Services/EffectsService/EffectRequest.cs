using UnityEngine;

namespace Slime.Services.EffectsService
{
    public class EffectRequest
    {
        public string ID { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Transform Parent { get; }
        public Vector3 Target { get; private set; }
        public float Lifetime { get; private set; } = 2f;
        public float Delay { get; private set; } = 0f;

        public EffectRequest(string id, Vector3 position, Quaternion rotation = default, Transform parent = null)
        {
            ID = id;
            Position = position;
            Rotation = rotation;
            Parent = parent;
        }

        public EffectRequest(string id, Transform parent)
        {
            ID = id;
            Position = parent.position;
            Rotation = parent.rotation;
            Parent = parent;
        }

        public EffectRequest SetTarget(Vector3 targetPosition)
        {
            Target = targetPosition;
            return this;
        }

        public EffectRequest SetLifetime(float lifetime)
        {
            Lifetime = lifetime;
            return this;
        }
        
        public EffectRequest SetDelay(float delay)
        {
            Delay = delay;
            return this;
        }
    }
}