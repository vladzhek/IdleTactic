using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using UI.Base.MVVM;
using UI.Base.Widgets;

namespace Slime.UI.Common.Tabbar
{
    public abstract class TabbarViewModel<TData> : ViewModel
    where TData : LayoutData<TData>
    {
        protected abstract ITabbarUIModel TabbarUIModel { get; }
        private List<TData> _tabsData = new();

        #region ViewModel implementation
        
        public override void OnSubscribe()
        {
            TabbarUIModel.OnTabChange += OnIndexChanged;
        }

        public override void OnUnsubscribe()
        {
            TabbarUIModel.OnTabChange -= OnIndexChanged;
        }
        
        public override void OnDispose()
        {
            _tabsData = null;
        }
        
        #endregion

        // ReSharper disable once MemberCanBeProtected.Global
        public virtual bool CanTabClose { get; protected set; } = true;
        public Action<int, TData> OnTabChange;
        
        public void SetActiveIndex(int index)
        {
            //Logger.Log($"index: {index}; already opened: {TabbarUIModel.OpenedTab == index}");
            
            if (CanTabClose && TabbarUIModel.SelectedTab == index)
            {
                TabbarUIModel.DeselectTab();
            }
            else
            {
                TabbarUIModel.SelectTab(index);
            }
        }
        
        public IEnumerable<TData> Data
        {
            get => _tabsData;
            protected set {
                foreach (var item in value)
                {
                    _tabsData.Add(item);
                }
            }
        }

        protected TData GetTabData(int index)
        {
            return _tabsData[index];
        }

        protected virtual void OnIndexChanged(int index, bool isOpen)
        {
            //Logger.Log($"index {index} open {isOpen}");
            
            for (var i = 0; i < _tabsData.Count; i++)
            {
                var data = _tabsData[i];
                var shouldOpen = isOpen && index == i;
                if (data.IsSelected == shouldOpen)
                {
                    continue;
                }
                
                //Logger.Log($"index: {i}; should open: {shouldOpen}");
                
                data.IsSelected = shouldOpen;
                OnTabChange?.Invoke(i, data);
            }
        }
    }
}