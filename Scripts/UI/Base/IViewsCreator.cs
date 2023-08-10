using System;
using System.Collections.Generic;
using UI.Base.MVVM;

namespace UI.Base
{
    public interface IViewsCreator
    {
        public bool IsInitialized { get; }
        public void Initialize(ViewsHolder viewsHolder, IEnumerable<CanvasSetup> canvasSetups);
        public void AddCanvas(CanvasSetup canvasSetup);
        public void RemoveCanvas(CanvasSetup canvasSetup);
        public T CreateView<T>() where T : ViewBase;
        public void CloseView<T>() where T : ViewBase;
        public void CloseLayer(UILayer layer);

        IEnumerable<ViewBase> ActiveViewsInLayer(UILayer layer);
    }
}