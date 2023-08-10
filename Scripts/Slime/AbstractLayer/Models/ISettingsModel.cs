using System;

namespace Slime.AbstractLayer.Models
{
    public interface ISettingsModel
    {
        public event Action<string> OnChange;
        
        public void Set(string key, object value);
        public string Get(string key, string defaultValue = null);
        public float GetFloat(string key, float defaultValue);
        public int GetInt(string key, int defaultValue);
        public bool GetBool(string key);
        TEnum GetEnum<TEnum>(string key, TEnum defaultValue = default) where TEnum : struct;
        DateTime GetDateTime(string key);
    }
}