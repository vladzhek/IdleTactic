using System;
using Slime.AbstractLayer.Models;

namespace Slime.Models
{
    public class FooterUIModel : IFooterUIModel
    {
        public event Action<int, bool> OnTabChange;
        public int SelectedTab { get; private set; } = -1;
        public bool IsTabSelected { get; private set; }

        public void SelectTab(int index)
        {
            //Logger.Log($"index {index}");
            
            SelectedTab = index;
            IsTabSelected = true;
            
            OnTabChange?.Invoke(SelectedTab, IsTabSelected);
        }

        public void DeselectTab()
        {
            var lastTab = SelectedTab;
            
            SelectedTab = -1;
            IsTabSelected = false;
            
            OnTabChange?.Invoke(lastTab, IsTabSelected);
        }
    }
}