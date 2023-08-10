using System;
using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using Slime.Configs.Setting;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Slime.Services
{
    public class AudioService : MonoBehaviour
    {
        private ISettingsModel _settingsModel;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioMixer _mixer;

        private readonly Dictionary<EAudioSources, AudioSource> _audioSources = new();
        private AudioConfig _audioConfig;
        
        private const string MUSIC = "MusicVolume";
        private const string SFX = "SFXVolume";

        [Inject]
        private void Construct(ISettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
        }
        
        public void Play(string id)
        {
            _audioConfig ??= LoadConfig();

            var data = _audioConfig.items.Find(x => x.ID == id);
            var type = data.AudioType;
            var clip = data.clip;

            if (type == EAudioSources.SFXSource)
                _audioSources[type].PlayOneShot(clip);
            else
                _audioSources[type].clip = clip;
        }

        private void Awake()
        {
            CreateAudioSource();
        }

        private void OnEnable()
        {
            _settingsModel.OnChange += OnSettingChanged;
        }
        
        private void OnDisable()
        {
            _settingsModel.OnChange -= OnSettingChanged;
        }
        
        private void CreateAudioSource()
        {
            _audioSources.Add(EAudioSources.MusicSource, _musicSource);
            _audioSources.Add(EAudioSources.SFXSource, _sfxSource);
        }

        private void OnSettingChanged(string id)
        {
            if (id is not (MUSIC or SFX)) return;

            _audioConfig ??= LoadConfig();
            var value = _settingsModel.GetBool(id) ? 0f : -80f;
            _mixer.SetFloat(id, value);
        }

        private static AudioConfig LoadConfig()
        {
            var path = $"{Data.Constants.System.CONFIG_PATH}Audio";
            var config = Resources.Load<AudioConfig>(path);
            if (!config)
            {
                throw new Exception($"no config at {path}");
            }

            return config;
        }
    }
}