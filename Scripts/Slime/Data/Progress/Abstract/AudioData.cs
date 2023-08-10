using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.Data.IDs;
using Slime.Services;
using UnityEngine;

namespace Slime.Data.Progress.Abstract
{
    [Serializable]
    public class AudioData
    {
        [Header("AudioData")]
        public EAudioSources AudioType;
        public string ID { 
            get => _id;
            set => _id = value;
        }

        public AudioClip clip;
        
        
        [SerializeField] [ValueDropdown(nameof(IDs))] private string _id;
        private IEnumerable<string> IDs => AudioIDs.Values;
    }
}