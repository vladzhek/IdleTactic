using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Popups.Settings
{
    // TODO: make generic
    public class Toggle : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Animator _toggle;
        [SerializeField] private Button _clickArea;

        public event Action<string,bool> OnChange;
        
        private string _statusOn = "ON";
        private string _statusOff = "OFF";
        
        private int _toggleOff;
        private int _toggleOn;

        private bool _isOn = true;
        private string toggleID = "default";
        
        private void Start()
        {
            _toggleOff = Animator.StringToHash("AudioToggleOff");
            _toggleOn = Animator.StringToHash("AudioToggleOn");
            _clickArea.onClick.AddListener(ToggleState);
        }

        public void SetStatus(string id, bool isActive)
        {
            _isOn = isActive;
            toggleID = id;
            
            if (isActive)
            {
                SetOn();
            }
            if (!isActive)
            {
                SetOff();
            }
        }
        
        public void SetStatusText(string textOn, string textOff)
        {
            _statusOn = textOn;
            _statusOff = textOff;
        }

        public void SetOn()
        {
            _toggle.SetBool("IsOn", true);
            _toggle.Play(_toggleOn);
            
            UpdateState(true);
        }

        public void SetOff()
        {
            _toggle.SetBool("IsOn", false);
            _toggle.Play(_toggleOff);
            
            UpdateState(false);
        }

        private void UpdateState(bool isOn)
        {
            _text.text = isOn ? _statusOn : _statusOff;
        }

        private void ToggleState()
        {
            if (_isOn)
            {
                SetOff();
                _isOn = false;
                OnChange?.Invoke(toggleID, false);
            }
            else
            {
                SetOn();
                _isOn = true;
                OnChange?.Invoke(toggleID, true);
            }
        }
    }
}