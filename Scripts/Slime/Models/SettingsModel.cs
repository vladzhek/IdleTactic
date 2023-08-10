using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using Slime.Models.Abstract;

namespace Slime.Models
{
    public class SettingsModel : BaseProgressModel, ISettingsModel
    {
        #region ISettingsModel implementation

        public event Action<string> OnChange;
        
        public void Set(string key, object value)
        {
            SettingsData[key] = value.ToString();
            OnChange?.Invoke(key);
        }

        public bool GetBool(string key)
        {
            var value = SettingsData.GetValueOrDefault(key);
            return value switch
            {
                "" => false,
                "0" => false,
                "False" => false,
                "false" => false,
                "null" => true,
                "Null" => true, 
                null => true, // NOTE: Active sounds in first enter 
                _ => true
            };
        }
        
        public int GetInt(string key, int defaultValue = -1)
        {
            var value = SettingsData[key];
            return int.TryParse(value, out var result) ? result : defaultValue;
        }
        
        public float GetFloat(string key, float defaultValue = -1)
        {
            var value = SettingsData[key];
            return float.TryParse(value, out var result) ? result : defaultValue;
        }
        
        public string Get(string key, string defaultValue = null)
        {
            if (SettingsData.TryGetValue(key, out string value))
            {
                return value;
            }
            
            return SettingsData[key] = defaultValue;
        }
        
        public TEnum GetEnum<TEnum>(string key, TEnum defaultValue = default) where TEnum : struct
        {
            return Enum.TryParse<TEnum>(Get(key), out var stage)
                ? stage : defaultValue;
        }

        public DateTime GetDateTime(string key)
        {
            var value = Get(key);
            if (string.IsNullOrEmpty(value)) return DateTime.MinValue;
            return DateTime.TryParse(value, out var result) ? result : DateTime.MinValue;
        }

        #endregion
        
        // private
        
        private SettingsModel(IGameProgressModel progressModel) : base(progressModel)
        {
        }

        private Dictionary<string, string> SettingsData
        {
            get
            {
                var data = GameData.SettingsData;
                if (data == null)
                {
                    data = new Dictionary<string, string>();
                    GameData.SettingsData = data;
                }

                return data;
            }
        }

        protected override void OnProgressLoaded()
        {
            foreach (var (key,value) in SettingsData)
            {
                OnChange?.Invoke(key);
            }
        }
    }
}