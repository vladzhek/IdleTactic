using System;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;

namespace Slime.Models
{
    [UsedImplicitly]
    public class CharacterUIModel : ICharacterUIModel
    {
        #region ICHaracterUIModel implementation
            
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
        
        #endregion
    }
}