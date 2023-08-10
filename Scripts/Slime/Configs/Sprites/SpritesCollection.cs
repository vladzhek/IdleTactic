using System.Collections.Generic;
using Slime.Data;
using UnityEngine;

// TODO: replace with config?
namespace Slime.Configs.Sprites
{
    [CreateAssetMenu(fileName = "Sprites", menuName = "Assets/SpritesCollection", order = 0)]
    public class SpritesCollection : ScriptableObject
    {
        [SerializeField] private SpriteEntry[] _spriteEntries;

        public IEnumerable<SpriteEntry> SpriteEntries => _spriteEntries;
    }
}