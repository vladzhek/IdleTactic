using UI;
using UI.Base;
using UnityEngine;
using Zenject;

namespace Slime.UI
{
    public class UIRouter : MonoBehaviour
    {
        [SerializeField] private ViewsHolder _viewsHolder;
        [SerializeField] private CanvasSetup[] _canvasSetup;

        private IViewsCreator _viewsCreator;

        [Inject]
        private void Construct(IViewsCreator viewsCreator)
        {
            _viewsCreator = viewsCreator;
            _viewsCreator.Initialize(_viewsHolder, _canvasSetup);
        }
    }
}