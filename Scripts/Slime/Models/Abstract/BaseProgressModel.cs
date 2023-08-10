using System;
using Slime.AbstractLayer.Models;
using Slime.Data.Progress;
using Zenject;

namespace Slime.Models.Abstract
{
    public abstract class BaseProgressModel : IInitializable, IDisposable
    {
        public virtual void Initialize()
        {
            if (ProgressModel.IsLoaded)
            {
                OnProgressLoaded();
            }
            else
            {
                ProgressModel.OnLoad += OnProgressLoaded;
            }
        }

        public virtual void Dispose()
        {
            
        }
        
        // private

        protected IGameProgressModel ProgressModel { get; }

        protected BaseProgressModel(IGameProgressModel progressModel)
        {
            ProgressModel = progressModel;
        }

        protected virtual void OnProgressLoaded()
        {
            ProgressModel.OnLoad -= OnProgressLoaded;
        }
        
        protected GameData GameData
        {
            get
            {
                var gameData = ProgressModel?.Get();
                if (gameData == null)
                {
                    throw new Exception("can't retrieve game data");
                }
                return gameData;
            }
        }
    }
}