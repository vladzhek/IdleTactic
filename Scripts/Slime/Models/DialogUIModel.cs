using Cysharp.Threading.Tasks;
using Slime.AbstractLayer.Models;
using Slime.Data.Dialog;
using Slime.UI;
using Slime.UI.Popups.Dialogs;

namespace Slime.Models
{
    public class DialogUIModel : IDialogUIModel
    {
        #region IIDialogUIModel implementation
        
        public DialogData Get()
        {
            return _data;
        }

        public UniTask Open(DialogData data)
        {
            _taskCompletionSource?.TrySetResult(false);
            _taskCompletionSource = new UniTaskCompletionSource<bool>();
            
            _data = data;
            _uiManager.Open<ConfirmView>();

            return _taskCompletionSource.Task;
        }

        public void Confirm()
        {
            _iSConfirmed = true;
            Close();
        }

        public void Cancel()
        {
            _iSConfirmed = false;
            Close();
        }

        public void Close()
        {
            _uiManager.Close<ConfirmView>();
            _taskCompletionSource.TrySetResult(_iSConfirmed);

            _data = null;
            _iSConfirmed = false;
            _taskCompletionSource = null;
        }
        
        #endregion
        
        // private

        private readonly UIManager _uiManager;
        
        private DialogData _data;
        private bool _iSConfirmed;
        private UniTaskCompletionSource<bool> _taskCompletionSource;
        
        private DialogUIModel(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
    }
}