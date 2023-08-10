using Slime.AbstractLayer.Battle;
using Utils;

namespace Slime.Gameplay.Battle.Skills.Abstract
{
    public abstract class Skill : ISkill
    {
        public abstract string ID { get; set; }
        public abstract float SecondsToCooldown { get; set; }
        public float Cooldown { get; set; }
        public bool IsReady { get; set; }

        public virtual void SetID(string id)
        {
            ID = id;
        }

        public virtual void Activate()
        {
            Cooldown = 0;
            IsReady = false;
            //Logger.Log($"{ID} skill activated");
        }

        public virtual void Update(float deltaTime)
        {
            Cooldown += deltaTime;

            if (Cooldown > SecondsToCooldown)
            {
                IsReady = true;
            }
        }
    }
}