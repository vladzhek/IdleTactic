using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Slime.AbstractLayer.Models
{
    public interface IOfflineIncomeModel : IInitializable, IDisposable
    {
        event Action<int> OnOfflineIncome;
        int OfflineIncomeValue { get; }
       void ShowAd();
       void AddReward(int value);
    }
}