using UnityEngine;

namespace UI.Base.MVVM
{
    public abstract class ViewBase : MonoBehaviour
    {
        public abstract UILayer Layer { get; }
        public virtual CanvasSetup? CanvasSetup => null;
    }
}