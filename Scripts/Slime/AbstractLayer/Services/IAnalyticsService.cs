

using System.Collections.Generic;

namespace Slime.Services.Analytics
{
    public interface IAnalyticsService 
    {
        void Send(EEventType type, Dictionary<string, object> parameters);
        void Register();
    }

    public interface IAnalyticsItemService : IAnalyticsService
    {
    }
}