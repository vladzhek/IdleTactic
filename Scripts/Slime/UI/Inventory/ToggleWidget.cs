using UnityEngine;

namespace Slime.UI.Inventory
{
    public class ToggleWidget : MonoBehaviour
    {
        // public
        
        public void SetActive(bool isActive = true)
        {
            _isInactive = false;
            _isActive = isActive;

            UpdateState();
        }

        public void SetInactive()
        {
            _isInactive = true;
            
            UpdateState();
        }
        
        // private
        
        [SerializeField] private GameObject _activeObject;
        [SerializeField] private GameObject _passiveObject;

        private bool _isActive;
        private bool _isInactive;

        private void Awake()
        {
            UpdateState();
        }
        
        private void UpdateState()
        {
            if (_activeObject) _activeObject.SetActive(!_isInactive && _isActive);
            if (_passiveObject) _passiveObject.SetActive(!_isInactive && !_isActive);
        }
    }
}