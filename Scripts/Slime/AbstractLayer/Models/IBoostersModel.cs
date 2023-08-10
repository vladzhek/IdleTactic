using System;
using System.Collections.Generic;
using Zenject;

namespace Slime.AbstractLayer.Models
{
    public interface IBoostersModel : IInitializable, IDisposable
    {
        event Action OnChange;
        event Action<string> Activated;
        event Action<string> Deactivated;

        Dictionary<string, IBooster> Boosters { get; }

        void ActivateBoosterForAds(string id);
    }
}