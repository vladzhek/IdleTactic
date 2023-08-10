using Slime.AbstractLayer.Configs;
using UnityEngine;

namespace Slime.Levels
{
    [CreateAssetMenu(fileName = "Environment", menuName = "Assets/Configs/Environment", order = 0)]
    public class EnvironmentConfig : ScriptableObject, IEnvironmentConfig
    {
        [SerializeField] private Sprite _background;

        public Sprite Background => _background;
    }
}