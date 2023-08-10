using Utils.UpdateLoops;

namespace Services
{
    public interface IUpdateInLoop
    {
        public EUpdateLoop UpdateLoop { get; }
        public bool IsActive { get; }

        public void Update(float deltaTime);
    }
}