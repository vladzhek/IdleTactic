using Slime.AbstractLayer.Models;
using UI.Base.MVVM;

namespace Slime.UI.Popups.Settings
{
    public class SettingsViewModel : ViewModel
    {
        private ISettingsModel _settingsModel;
        private UIManager _uiManager;
        
        SettingsViewModel(ISettingsModel settingsModel, UIManager uiManager)
        {
            _uiManager = uiManager;
            _settingsModel = settingsModel;
        }

        public void ChangeAudio(string id, bool isActive)
        {
            _settingsModel.Set(id, isActive);
        }

        public bool GetAudioActive(string id)
        {
            return _settingsModel.GetBool(id);
        }

        public void CloseView()
        {
            _uiManager.Close<SettingsView>();
        }
    }
}