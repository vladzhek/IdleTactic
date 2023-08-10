using System;
using JetBrains.Annotations;
using ModestTree;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.Data.Enums;
using Utils;
using Utils.Extensions;
using Utils.Time;
using Zenject;

namespace Slime
{
    // NOTE: enforce/warn about "using"?
    // NOTE: user is not aware Dispose needs to be called
    // NOTE: user is not aware that if NeedsRefresh is true,
    // refresher is automatically confirming refresh was done
    [UsedImplicitly]
    public class DailyEntityRefresher : IEntityRefresher
    {
        public bool NeedsRefresh { get; private set; }

        public void Dispose()
        {
            if (NeedsRefresh)
            {
                _settingsModel.Set(Key, _currentRefreshTime);
            }
        }
        
        [UsedImplicitly]
        public class Factory : PlaceholderFactory<ERefreshableEntity, DailyEntityRefresher>
        {
        }
        
        // private
        
        private const string KEY_SUFFIX = "lastRefresh";
        private readonly ERefreshableEntity _entity;
        private readonly ISettingsModel _settingsModel;
        private readonly DateTime _currentRefreshTime;
        
        private string Key => $"{_entity.ToString()}{KEY_SUFFIX.Capitalize()}";

        private DailyEntityRefresher(ERefreshableEntity entity,
            ISettingsModel settingsModel,
            ITimeService timeService)
        {
            _entity = entity;
            _settingsModel = settingsModel;
            _currentRefreshTime = timeService.CurrentTime;
            
            Initialize();
        }

        private void Initialize()
        {
            var lastRefreshTime = _settingsModel.GetDateTime(Key);
            if (lastRefreshTime == DateTime.MinValue)
            {
                Logger.Log($"entity: {_entity}; had not been refreshed yet");
                NeedsRefresh = true;
                return;
            }

            var span = _currentRefreshTime - lastRefreshTime; 
            if (span.Days > 0)
            {
                Logger.Log($"entity: {_entity}; had been refreshed {span.Days} day(s) ago");
                NeedsRefresh = true;
                return;
            }
            
            Logger.Log($"entity: {_entity}; has been refreshed today");
        }
    }
}