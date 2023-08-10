namespace Slime.AbstractLayer.Battle
{
    public interface ISkill
    {
        public string ID { get; }
        public float Cooldown { get; }
        public float SecondsToCooldown { get; set; }
        public bool IsReady { get; }
        public void Activate();
        public void Update(float deltaTime);
        public void SetID(string id);
    }
}