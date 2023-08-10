using TMPro;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Tutorial
{
    public class TutorialView : View<TutorialViewModel>
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        
        public override UILayer Layer => UILayer.Overlay;

        protected override void OnEnable()
        {
            base.OnEnable();

            ViewModel.OnDisplayMessage += DisplayedMessage;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            ViewModel.OnDisplayMessage -= DisplayedMessage;
        }

        private void DisplayedMessage(string message)
        {
            _messageText.transform.parent.gameObject.SetActive(message != null);
            _messageText.text = message;
        }
    }
}