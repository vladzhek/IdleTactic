using Slime.Data.Abstract;

namespace Slime.Data.Dialog
{
    public class DialogData : GenericData
    {
        public readonly string Title;
        public readonly string Description;
        public readonly bool CanCancel;
        public readonly string ConfirmTitle;
        public readonly string CancelTitle;

        public DialogData(string title, 
            string description = null, 
            bool canCancel = false, 
            string confirmTitle = "confirm", 
            string cancelTitle = "cancel")
        {
            Title = title;
            Description = description;
            CanCancel = canCancel;
            ConfirmTitle = confirmTitle;
            CancelTitle = cancelTitle;
        }
    }
}