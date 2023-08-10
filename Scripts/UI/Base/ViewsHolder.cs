using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI.Base.MVVM;
using UnityEngine;

namespace UI.Base
{
    [CreateAssetMenu(fileName = "ViewsHolder", menuName = "Assets/ViewsHolder")]
    public class ViewsHolder : ScriptableObject
    {
        [SerializeField, AssetSelector(Paths = "Assets/Prefabs/UI/Views")] 
        private List<ViewBase> _views;
        
        public List<ViewBase> Views => _views;
    }
}