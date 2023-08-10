using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Slime.AbstractLayer;
using UI.Base.MVVM;
using UnityEngine;
using Logger = Utils.Logger;
using Object = UnityEngine.Object;

namespace UI.Base
{
    [UsedImplicitly]
    public class ViewsCreator : IViewsCreator
    {
        // dependencies
        private readonly IObjectFactory _objectFactory;

        // state
        private readonly Dictionary<Type, ViewBase> _views = new();
        private readonly Dictionary<UILayer, Canvas> _canvases = new();

        private readonly List<Type> _activeViewsTypes = new();
        private readonly Dictionary<Type, ViewBase> _activeViews = new();
        private readonly Dictionary<UILayer, List<ViewBase>> _activeViewsByLayer = new();

        private ViewsCreator(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        private void RemoveFromLayer(ViewBase viewBase)
        {
            _activeViewsByLayer.GetValueOrDefault(viewBase.Layer)?.Remove(viewBase);
        }
        
        #region IViewsCreator implementation
        
        public bool IsInitialized { get; private set; }
        
        public void Initialize(ViewsHolder viewsHolder, IEnumerable<CanvasSetup> canvasSetups)
        {
            foreach (var view in viewsHolder.Views)
            {
                _views[view.GetType()] = view;
            }

            foreach (var canvasSetup in canvasSetups)
            {
                _canvases[canvasSetup.Layer] = canvasSetup.Canvas;
            }
            
            IsInitialized = true;
        }

        public void AddCanvas(CanvasSetup canvasSetup)
        {
            _canvases[canvasSetup.Layer] = canvasSetup.Canvas;
        }

        public void RemoveCanvas(CanvasSetup canvasSetup)
        {
            _canvases.Remove(canvasSetup.Layer);
        }
        
        public T CreateView<T>() where T : ViewBase
        {
            var type = typeof(T);
            
            //Logger.Log($"type: {type}");

            if (!_views.TryGetValue(type, out var prefab))
            {
                Logger.Error($"no prefab for view {type}");
                return null;
            }

            if (!_canvases.TryGetValue(prefab.Layer, out var canvas))
            {
                Logger.Error($"no canvas for layer {prefab.Layer}");
                return null;
            }
            
            if (!canvas)
            {
                Logger.Error($"canvas is null!");
                return null;
            }

            try
            {
                return CreateView<T>(prefab, canvas);
            }
            catch (Exception e)
            {
                Logger.Error($"type: {type}; {e}");
                throw;
            }
        }

        private T CreateView<T>(ViewBase prefab, Component canvas) where T : ViewBase
        {
            var type = typeof(T);
            //var type = default(T)!.GetType();
            
            if (!canvas)
            {
                Logger.Error($"canvas is null!");
                return null;
            }

            if (_activeViews.ContainsKey(type))
            {
                Logger.Log($"view {type} already exists");
                CloseView<T>();
            }

            var view = _objectFactory.CreateObject(prefab, canvas.transform) as T;
            var layers = _activeViewsByLayer.GetValueOrDefault(view!.Layer);
            if (layers == null)
            {
                layers = new List<ViewBase>();
                _activeViewsByLayer.Add(view.Layer, layers);
            }
 
            layers.Add(view);
            _activeViews[type] = view;
            if (_activeViewsTypes.Contains(type))
            {
                _activeViewsTypes.Add(type);
            }

            var personalCanvas = view.CanvasSetup;
            if (personalCanvas != null)
            {
                AddCanvas(personalCanvas.Value);
            }

            return view;
        }

        public void CloseView<T>() where T : ViewBase
        {
            var viewType = typeof(T);
            if (_activeViews.TryGetValue(viewType, out var view))
            {
                var personalCanvas = view.CanvasSetup;
                if (personalCanvas != null)
                {
                    RemoveCanvas(personalCanvas.Value);
                }

                Object.DestroyImmediate(view);
                _activeViews.Remove(viewType);
                RemoveFromLayer(view);
            }

            if (_activeViewsTypes.Contains(viewType))
            {
                _activeViewsTypes.Remove(viewType);
            }
        }

        public void CloseLayer(UILayer layer)
        {
            if (_activeViewsByLayer.TryGetValue(layer, out var views))
            {
                foreach (var viewBase in views)
                {
                    Object.Destroy(viewBase);
                }
            }

            _activeViewsByLayer[layer].Clear();
        }

        public IEnumerable<ViewBase> ActiveViewsInLayer(UILayer layer)
        {
            return _activeViewsByLayer[layer];
        }
        
        #endregion
    }
}