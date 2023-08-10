using System;
using Slime.Data.Abstract;
using Slime.Data.Progress;

namespace Slime.AbstractLayer.Models
{
    public interface IGameProgressModel : IProgressModel<GameData, GenericData>
    {
        public bool IsLoaded { get; }
        public event Action OnLoad;
    }
}