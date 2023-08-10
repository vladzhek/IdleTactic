using System;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Tutorial
{
    public class TutorialViewModel : ViewModel
    {
        private readonly ITutorialModel _tutorialModel;

        private TutorialViewModel(ITutorialModel tutorialModel)
        {
            _tutorialModel = tutorialModel;
        }
        
        public event Action<string> OnDisplayMessage
        {
            add => _tutorialModel.OnDisplayMessage += value;
            remove => _tutorialModel.OnDisplayMessage -= value;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnSubscribe()
        {
            base.OnSubscribe();
        }

        public override void OnUnsubscribe()
        {
            base.OnUnsubscribe();
        }
    }
}