using Cysharp.Threading.Tasks;
using Slime.Data.Dialog;

namespace Slime.AbstractLayer.Models
{
    public interface IDialogUIModel
    {
        public DialogData Get();
        public UniTask Open(DialogData data);
        public void Confirm();
        public void Cancel();
        public void Close();
    }
}