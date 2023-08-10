using System;
using Cysharp.Threading.Tasks;
using MAXHelper;
using ModestTree;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Services;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.Services
{
    public class AdsService : MonoBehaviour, IAdsService
    {
        public UniTask<bool> ShowAd(string placement)
        {
            if (!IsReady)
            {
                Logger.Warning($"mad pixel ads service is not ready");
                return UniTask.FromResult(false);
            }
            
            // DEBUG: temporarily return true
            return UniTask.FromResult(true);

            _taskCompletionSource = new UniTaskCompletionSource<bool>();

            var result = AdsManager.ShowRewarded(gameObject, OnFinishAds, placement);
            if (result != AdsManager.EResultCode.OK)
            {
                _taskCompletionSource = null;
                return UniTask.FromResult(false);
            }
            
            return _taskCompletionSource.Task;
        }

        private void OnFinishAds(bool success)
        {
            if (_taskCompletionSource != null)
            {
                _taskCompletionSource.TrySetResult(success);
                _taskCompletionSource = null;
            }
        }
        
        // private
        
        private UniTaskCompletionSource<bool> _taskCompletionSource;
        private bool _isInit;

        private bool IsReady => AdsManager.Exist && _isInit;
        
        private void Awake()
        {
            // NOTE: add some kind of no internet protection
            if (AdsManager.Exist)
            {
                AdsManager.Instance.InitApplovin();
            }
            _isInit = true;
        }
    }
}