namespace UI.Base.MVVM
{
    public abstract class ViewModel
    {
        public virtual void OnInitialize() {}
        public virtual void OnStart() {}
        public virtual void OnEnable() {}
        public virtual void OnDisable() {}
        public virtual void OnSubscribe() {}
        public virtual void OnUnsubscribe() {}
        public virtual void OnDispose() {}
    }
}