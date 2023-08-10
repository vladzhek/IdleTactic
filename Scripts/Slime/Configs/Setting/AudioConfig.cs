using System.Collections.Generic;
using Sirenix.OdinInspector;
using Slime.Configs.Abstract;
using Slime.Configs.Inventory;
using Slime.Data.Inventory;
using Slime.Data.Progress.Abstract;
using UnityEngine;

namespace Slime.Configs.Setting
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class AudioConfig : ScriptableObject
    {
        private const string ENTITY = "Audio";
        
        [SerializeField] public List<AudioData> items = new List<AudioData>();
    }
}