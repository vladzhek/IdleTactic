using System;
using System.IO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Slime.AbstractLayer.Services;
using Slime.Data.Constants;
using Slime.Data.Progress;
using Slime.Exceptions;
using UnityEngine;
using Constants = Slime.Data.Constants.System;
using Logger = Utils.Logger;

namespace Slime.Services
{
    [UsedImplicitly]
    public class SaveService : ISaveService
    {
        // NOTE: JsonUtility ignores nullable types
        // NOTE: working with file can be moved to separate service
        // NOTE: backup save file
        // NOTE: multiple save files
        
        private static string GetPath()
        {
            var projectPath = Application.persistentDataPath;
            var directoryPath = Constants.SAVE_PATH
                .Replace("/", $"{Path.DirectorySeparatorChar}");
            var filename = $"{Constants.SAVE_NAME}.{Constants.SAVE_EXTENSION}";
            return Path.Combine(projectPath, directoryPath, filename);
        }

        private void SaveDataToPath(GameData data)
        {
            //Logger.Log($"data: {data}");
            
            var path = GetPath();
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
               Directory.CreateDirectory(directory);
            }

            using var stream = new FileStream(path, FileMode.Create);
            using var writer = new StreamWriter(stream);
            
            //var serializedData = JsonUtility.ToJson(data, true);
            var serializedData = JsonConvert.SerializeObject(data, Formatting.Indented);
            Logger.Log($"serialized data: {serializedData}");
            writer.Write(serializedData);
        }
        
        private GameData LoadDataFromPath()
        {
            var path = GetPath();
            if (!File.Exists(path))
            {
                Logger.Warning($"no save at {path}");
                throw new ProgressLoadException(Slime.Data.Constants.Exceptions.PROGRESS_FILE_NOT_EXISTS);
            }

            using var stream = new FileStream(path, FileMode.Open);
            using var reader = new StreamReader(stream);
            
            var serializedData = reader.ReadToEnd();
            //Logger.Log($"serialized data: {serializedData}");
            if (string.IsNullOrEmpty(serializedData))
            {
                throw new ProgressLoadException(Slime.Data.Constants.Exceptions.PROGRESS_FILE_EMPTY);
            }
            
            //var data = JsonUtility.FromJson<GameData>(serializedData);
            var data = JsonConvert.DeserializeObject<GameData>(serializedData);
            if (data == null)
            {
                throw new ProgressLoadException(Slime.Data.Constants.Exceptions.PROGRESS_FILE_CORRUPTED);
            }
            
            return data;
        }
        
        #region ISaveService implementation
        
        public UniTask Save(GameData data)
        {
            //Logger.Warning();
            
            try
            {
                SaveDataToPath(data);
                return UniTask.CompletedTask;
            }
            catch (Exception e)
            {
                Logger.Error($"{e}");
                throw new ProgressSaveException();
            }
        }
        
        public UniTask<GameData> Load()
        {
            //Logger.Warning();
            
            try
            {
                var data = LoadDataFromPath();
                return UniTask.FromResult(data);
            }
            catch (Exception e)
            {
                Logger.Warning($"{e}");
                throw new ProgressLoadException();
            }
        }

        #endregion
    }
}