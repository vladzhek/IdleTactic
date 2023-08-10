using System;

namespace Slime.AbstractLayer.Models
{
    public interface ITabbarUIModel
    {
        public event Action<int, bool> OnTabChange;
        public int SelectedTab { get; }
        public bool IsTabSelected { get; }

        public void SelectTab(int index);
        public void DeselectTab();
    }
}