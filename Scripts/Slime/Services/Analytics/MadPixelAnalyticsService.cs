using System.Collections.Generic;
using Logger = Utils.Logger;
using Manager = MadPixelAnalytics.AnalyticsManager;

namespace Slime.Services.Analytics
{
    public class MadPixelsAnalyticsService : IAnalyticsItemService
    {
        public void Send(EEventType type, Dictionary<string, object> parameters)
        {
            var sendEventBuffer = type is EEventType.StageStart or EEventType.StageFinish;

            if (!IsReady)
            {
                Logger.Warning("mad pixel analytics service is not ready");
                return;
            }
            
            Manager.CustomEvent(type.ToMadPixelsEventID(), parameters , sendEventBuffer);
        }

        public void Register()
        {
            // NOTE: add some kind of no internet protection
            Manager.Instance.Init();
            Manager.Instance.SubscribeToAdsManager();
            _isInit = true;
        }

        private bool _isInit;
        private bool IsReady => Manager.Exist && _isInit;
    }
}