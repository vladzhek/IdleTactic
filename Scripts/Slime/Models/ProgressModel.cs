using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Services;
using Slime.Data.Abstract;
using Slime.Data.Progress;
using Slime.Exceptions;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.Models
{
    // TODO: refactor to DataModel with CRUD interface
    [UsedImplicitly]
    public class ProgressModel : IGameProgressModel
    {
        // dependencies
        private readonly ISaveService _saveService;

        // state
        private GameData _gameData;

        private ProgressModel(ISaveService saveService)
        {
            _saveService = saveService;
        }

        #region ISavable implementation

        public UniTask Save()
        {
            if (_gameData == null)
            {
                Logger.Warning("trying to save null data");
                return UniTask.FromResult(false);
            }
            
            Logger.Warning();

            _gameData.DateOfLastSession = DateTime.Now;
            
            return _saveService.Save(_gameData);
        }

        public async UniTask Load()
        {
            Logger.Warning();

            try
            {
                _gameData = await _saveService.Load();
            }
            catch (ProgressLoadException e)
            {
                Logger.Warning($"{e.Message}");
                _gameData = await Create();
            }
            catch (Exception e)
            {
                Logger.Error($"{e}");
                throw;
            }

            OnLoad?.Invoke();
        }

        #endregion

        #region IProgressModel implementation

        public UniTask<GameData> Create()
        {
            Logger.Warning();

            // TODO: get userId from separate model
            var userId = SystemInfo.deviceUniqueIdentifier;

            _gameData = new GameData(userId);
            return UniTask.FromResult(_gameData);
        }

        public GameData Get()
        {
            if (!IsLoaded)
            {
                Logger.Warning("game data is null");
                // if we want to load from here we need to be aware of multiple loading attempts
                //_ = Load();
            }

            return _gameData;
        }

        // NOTE: this doesn't look good, but we can potentially update based on type of GenericData and emmit event
        public void Update(GenericData data)
        {
            if (_gameData == null)
            {
                throw new Exception($"game data is null; {data.GetType()} trying to update too early");
            }

            switch (data)
            {
                /*
                case GameData gameData:
                    // deep reactive object?
                    // NOTE: use reflection? ref types? single element array means we add it or replace? 
                    // NOTE: default values needs to be removed too
                    if (gameData.ID != null)
                    {
                        _gameData.ID = gameData.ID;
                    }

                    if (gameData.EquippedCharacterID != null)
                    {
                        _gameData.EquippedCharacterID = gameData.EquippedCharacterID;
                    }

                    // NOTE: do not invoke if nothing was changed?
                    Updated?.Invoke(gameData);
                    break;*/
                default:
                    throw new Exception($"{data.GetType()} update not implemented in progress model");
            }
        }

        public event Action<GenericData> Updated;

        #endregion

        #region IGameProgressModel implementation

        public bool IsLoaded => _gameData != null;
        public event Action OnLoad;

        #endregion
    }
}