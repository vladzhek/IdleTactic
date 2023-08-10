namespace Slime.AbstractLayer.StateMachine
{
    public interface ITriggerResponder<TTrigger>
    {
        void FireTrigger(TTrigger trigger);
    }
}