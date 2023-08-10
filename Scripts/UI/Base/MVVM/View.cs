using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Slime.AbstractLayer;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils;
using Utils.Extensions;
using Zenject;

namespace UI.Base.MVVM
{
    public abstract class View<T> : ViewBase where T : ViewModel
    {
        private IObjectFactory _objectFactory;
        private T _viewModel;
        private ButtonInfo[] _buttonInfos;
        private readonly Dictionary<string,UnityAction> _callbacks = new();
        
        protected T ViewModel => _viewModel ??= _objectFactory.CreateObject<T>();
        protected IObjectFactory Factory => _objectFactory;

        [Inject]
        private void SetFactory(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        protected virtual void Awake()
        {
            ViewModel.OnInitialize();

            _buttonInfos = GetButtonInfos();
        }

        protected virtual void Start()
        {
            ViewModel.OnStart();
        }
        
        protected virtual void OnEnable()
        {
            OnSubscribe();
            ViewModel.OnSubscribe();
            ViewModel.OnEnable();
        }

        protected virtual void OnDisable()
        {
            OnUnsubscribe();
            ViewModel.OnUnsubscribe();
            ViewModel.OnDisable();
        }
        
        protected virtual void OnSubscribe()
        {
            if (_buttonInfos != null) foreach (var buttonInfo in _buttonInfos)
            {
                if (buttonInfo.Button) buttonInfo.Button.onClick.AddListener(GetCallback(buttonInfo));
            }
        }
        
        protected virtual void OnUnsubscribe()
        {
            if (_buttonInfos != null) foreach (var buttonInfo in _buttonInfos)
            {
                if (buttonInfo.Button) buttonInfo.Button.onClick.RemoveListener(GetCallback(buttonInfo));
            }
        }
        
        protected virtual void OnDestroy()
        {
            ViewModel.OnDispose();
        }

        private ButtonInfo[] GetButtonInfos()
        {
            var targetType = typeof(Button);
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            return GetType()
                    .GetFields(flags)
                    .Where(fieldInfo => targetType.IsAssignableFrom(fieldInfo.FieldType))
                    .Select(fieldInfo => new ButtonInfo()
                    {
                        Button = fieldInfo.GetValue(this) as Button,
                        Name = fieldInfo.Name
                    })
                    .ToArray();
        }

        private UnityAction GetCallback(ButtonInfo buttonInfo)
        {
            //Logger.Log($"name: {buttonInfo.Name}; button: {buttonInfo.Button}");
            
            var key = buttonInfo.Name;
            var callback = _callbacks.GetValueOrDefault(key);
            if (callback == null)
            {
                callback = () => OnButtonClicked(buttonInfo);
                _callbacks[key] = callback;
            }
            return callback;
        }

        private void OnButtonClicked(ButtonInfo buttonInfo)
        {
            var methodName = $"On{buttonInfo.Name.Replace("_", "").Capitalize()}Clicked";
            
            Logger.Log($"name: {buttonInfo.Name}; method: {methodName}");

            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            GetType().GetMethod(methodName, flags)?.Invoke(this, null);
            ViewModel.GetType().GetMethod(methodName, flags)?.Invoke(ViewModel, null);
        }
    }

    internal class ButtonInfo
    {
        public Button Button;
        public string Name;
    }
}