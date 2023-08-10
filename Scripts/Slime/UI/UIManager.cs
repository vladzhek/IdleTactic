using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.Data.Constants;
using Slime.UI.Attributes;
using Slime.UI.Footer;
using Slime.UI.Footer.Character;
using Slime.UI.Footer.Companions;
using Slime.UI.Footer.Dungeons;
using Slime.UI.Footer.Store;
using Slime.UI.Footer.Tabbar;
using Slime.UI.Header;
using Slime.UI.Health;
using Slime.UI.Store.Summon;
using UI.Base;
using UI.Base.MVVM;
using Utils;
using Utils.Extensions;
using Utils.Time;
using Logger = Utils.Logger;

namespace Slime.UI
{
    [UsedImplicitly]
    public class UIManager
    {
        // dependencies
        private readonly IViewsCreator _viewsCreator;
        private readonly TimerService _timerService;

        // state
        private Type _activeTab;
        private readonly Dictionary<Type, ViewBase> _views = new();

        private readonly Type[] _mainTabbarTypes =
        {
            typeof(CharacterTabbarView),
            typeof(CompanionsView),
            typeof(DungeonsEntryView),
            typeof(CastleView),
            typeof(StoreTabbarView)
        };

        private UIManager(IViewsCreator viewsCreator, IFooterUIModel footerUIModel, TimerService timerService)
        {
            _viewsCreator = viewsCreator;
            _timerService = timerService;

            footerUIModel.OnTabChange += OnFooterTabChange;
        }
        
        public void InitializeBaseViews()
        {
            SetVisible<BlackScreenView>();
            SetVisible<HeaderView>();
        }

        public void InitializeGameViews()
        {
            SetVisible<BattleView>();
            SetVisible<AttributesView>();
            SetVisible<HealthView>();
            SetVisible<FooterView>();
        }
        
        public void OpenWithTimer<T>(string id, int duration) where T : ViewBase
        {
            _timerService.CreateTimer(id, duration);
            Open<T>();
        }
        
        // NOTE: prefer calling generic method
        public void Open(Type viewType, bool shouldReplaceLayer = true)
        {
            //Logger.Warning($"prefer calling with generic <{viewType}>");
            this.CallGenericVersion(nameof(Open), viewType,
                new object[]{shouldReplaceLayer});
        }

        public void Open<T>(bool shouldReplaceLayer = true) where T : ViewBase
        {
            SetVisible<T>(true, shouldReplaceLayer);
        }
        
        // NOTE: prefer calling generic method
        public void Close(Type viewType, bool shouldReplaceLayer = true)
        {
            //Logger.Warning($"prefer calling with generic <{viewType}>");
            this.CallGenericVersion(nameof(Close), viewType,
                new object[]{shouldReplaceLayer});
        }
        
        public void Close<T>(bool shouldReplaceLayer = true) where T : ViewBase
        {
            SetVisible<T>(false, shouldReplaceLayer);
        }

        public bool IsActiveFooterTabbar()
        {
            foreach (var tabbarType in _mainTabbarTypes)
            {
                if (_views.TryGetValue(tabbarType, out var view))
                {
                    if (view.gameObject.activeSelf)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // NOTE: prefer calling generic method
        private void SetVisible(Type viewType, bool isVisible = true, bool shouldReplaceLayer = false) 
        {
            //Logger.Warning($"prefer calling with generic <{viewType}>");
            this.CallGenericVersion(nameof(SetVisible), viewType,
                new object[]{isVisible, shouldReplaceLayer});
        }
        
        private void SetVisible<T>(bool isVisible = true, bool shouldReplaceLayer = false) where T : ViewBase
        {
            var viewType = typeof(T);
            var view = GetView<T>();
            if (!view)
            {
                Logger.Error($"can't set view {viewType} visible");
                return;
            }

            if (shouldReplaceLayer)
            {
                var views = _viewsCreator.ActiveViewsInLayer(view.Layer).ToList();
                // NOTE: foreach causes error 
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var i = 0; i < views.Count; i++)
                {
                    var type = views[i].GetType();
                    if (viewType != type)
                    {
                        SetVisible(type, false);
                    }
                }
            }

            view.gameObject.SetActive(isVisible);
        }

        private T GetView<T>() where T : ViewBase
        {
            var viewType = typeof(T);
            
            //Logger.Log($"type: {viewType}");
            
            if (!_views.TryGetValue(viewType, out var viewBase))
            {
                viewBase = _viewsCreator.CreateView<T>();
                _views[viewType] = viewBase;
            }

            return viewBase as T;
        }
    

        // NOTE: prefer calling generic method
        private ViewBase GetView(Type viewType)
        {
            //Logger.Log($"type: {viewType}");
            //Logger.Warning($"prefer calling with generic <{viewType}>");
            
            return this.CallGenericVersion(nameof(GetView), viewType) as ViewBase;
        }
        
        private void OnFooterTabChange(int index, bool shouldOpen)
        {
            //Logger.Log($"index: {index}; shouldOpen: {shouldOpen}");
            
            var type = _mainTabbarTypes[index];
            if (shouldOpen)
            {
                Open(type);
            }
            else
            {
                Close(type);
                Open<AttributesView>();
            }
        }
    }
}