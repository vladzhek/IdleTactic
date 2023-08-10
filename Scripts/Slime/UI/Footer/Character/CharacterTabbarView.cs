using System.Linq;
using Slime.UI.Common;
using Slime.UI.Common.Tabbar;
using UI.Base;
using UI.Base.MVVM;
using UnityEngine;

namespace Slime.UI.Footer.Character
{
    public class CharacterTabbarView : View<CharacterTabbarViewModel>
    {
        [SerializeField] private Canvas _tabCanvas;
        [SerializeField] private TabbarWidget _tabbarWidget;
        public override UILayer Layer => UILayer.Middleground;

        public override CanvasSetup? CanvasSetup => new CanvasSetup
        {
            Layer = UILayer.CharacterTabbar,
            Canvas = _tabCanvas
        };

        protected override void Start()
        {
            base.Start();
            
            _tabbarWidget.SetData(ViewModel.Data);
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            
            _tabbarWidget.OnSelect += OnElementSelected;
            ViewModel.OnTabChange += OnTabChanged;
        }

        protected override void OnUnsubscribe()
        {
            base.OnUnsubscribe();

            _tabbarWidget.OnSelect -= OnElementSelected;
            ViewModel.OnTabChange -= OnTabChanged;
        }

        private void OnElementSelected(TabbarData data)
        {
            //Logger.Log($"index: {index}");
            
            ViewModel.SetActiveIndex(ViewModel.Data.ToList().IndexOf(data));
        }

        private void OnTabChanged(int index, TabbarData data)
        {
            _tabbarWidget.SetData(index, data);
        }
    }
}