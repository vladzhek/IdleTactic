using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Zenject;

namespace Slime.Services.Analytics
{
    public class AnalyticsManager : IAnalyticsManager, IInitializable, IDisposable
    {
        private readonly Dictionary<Type, IAnalyticsService> _analyticServicesByType = new();
        private readonly IAnalyticsService[] _analyticsServices;

        private AnalyticsManager(IAnalyticsItemService[] analyticsServices)
        {
            _analyticsServices = analyticsServices;
        }

        public void AddService(IAnalyticsService service)
        {
            var type = service.GetType();
            
            if (!_analyticServicesByType.ContainsKey(type))
            {
                _analyticServicesByType.Add(type, service);
            }
            else
            {
                return;
            }
        }

        public void RemoveService(Type type)
        {
            _analyticServicesByType.Remove(type);
        }

        public void Send(EEventType type, Dictionary<string, object> parameters)
        {
            foreach (var service in _analyticServicesByType.Values)
            {
                service.Send(type, parameters);
            }
        }

        public void Register()
        {
            foreach (var service in _analyticServicesByType.Values)
            {
                service.Register();
            }
        }

        public void Initialize()
        {
            Register();
            foreach (var service in _analyticsServices)
            {
                AddService(service);
            }
        }

        public void Dispose()
        {
            var types = _analyticServicesByType.Keys;
            for (var i = 0; i < types.Count; i++)
            {
                RemoveService(types.ElementAt(i));
            }
        }
    }
}