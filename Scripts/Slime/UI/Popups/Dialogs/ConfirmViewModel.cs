using Slime.AbstractLayer.Models;
using Slime.Data.Dialog;
using UI.Base.MVVM;

namespace Slime.UI.Popups.Dialogs
{
    public class ConfirmViewModel : ViewModel
    {
        public DialogData Data => _dialogUIModel.Get();

        public void RequestClose()
        {
            _dialogUIModel.Close();
        }

        public void RequestConfirm()
        {
            _dialogUIModel.Confirm();
        }

        public void RequestCancel()
        {
            _dialogUIModel.Cancel();
        }
        
        // private

        private readonly IDialogUIModel _dialogUIModel;

        private ConfirmViewModel(IDialogUIModel dialogUIModel)
        {
            _dialogUIModel = dialogUIModel;
        }
    }
}