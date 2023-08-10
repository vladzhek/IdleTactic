using System;
using System.Collections.Generic;
using Slime.Data.IDs;

namespace Slime.Services.Analytics
{
    public interface IAnalyticsManager : IAnalyticsService
    {
        void AddService(IAnalyticsService service);
        void RemoveService(Type type);
    }
}