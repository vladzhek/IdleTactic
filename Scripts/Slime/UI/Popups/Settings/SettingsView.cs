using UI.Base;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Popups.Settings
{
    public class SettingsView : View<SettingsViewModel>
    {
        [SerializeField] private PopupWidget _popupWidget;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Toggle _SFXToggle;
        
        private const string MUSIC = "MusicVolume";
        private const string SFX = "SFXVolume";
        public override UILayer Layer => UILayer.Overlay;

        protected override void OnEnable()
        {
            base.OnEnable();
            _popupWidget.SetScrollable(false);
            _popupWidget.SetTile("SETTINGS");
            _musicToggle.SetStatus(MUSIC, ViewModel.GetAudioActive(MUSIC));
            _SFXToggle.SetStatus(SFX, ViewModel.GetAudioActive(SFX));
        }

        private void CloseView()
        {
            ViewModel.CloseView();
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            _musicToggle.OnChange += OnSoundChange;
            _SFXToggle.OnChange += OnSoundChange;
            _popupWidget.OnCloseButtonClick += CloseView;
        }
        
        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
            _musicToggle.OnChange -= OnSoundChange;
            _SFXToggle.OnChange -= OnSoundChange;
            _popupWidget.OnCloseButtonClick -= CloseView;
        }

        private void OnSoundChange(string id, bool isActive)
        {
            ViewModel.ChangeAudio(id, isActive);
        }
    }
}