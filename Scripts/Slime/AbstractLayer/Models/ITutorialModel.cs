using System;
using Slime.Data.Enums;

namespace Slime.AbstractLayer.Models
{
    public interface ITutorialModel
    {
        event Action<string> OnDisplayMessage;
        event Action<ETutorialStage> OnChange;
        ETutorialStage Stage { get; }
        void SetTutorialType(ETutorialStage stage);
        void DisplayMessage(string message);
    }
}