using System;
using System.Collections.Generic;
using Slime.Configs.Rewards;
using Slime.Data.Enums;
using Zenject;

namespace Slime.UI.Footer.Dungeons
{
    public interface IDungeonsUIModel : IInitializable, IDisposable
    {
        // TODO: do we need this event?
        int GetAvailableKeysValue(EStageType type);
        int GetAvailableAdsValue(EStageType type);
        void LoadDungeon(EStageType type);
        bool IsDailyDungeonAdsAvailable(EStageType type);
        int GetDungeonLevel(EStageType type);
    }
}