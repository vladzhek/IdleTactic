using Slime.Configs.Abstract;
using Slime.Data.Characters;
using UnityEngine;

// TODO: proper names and path for ALL configs

namespace Slime.Configs.Characters
{
    [CreateAssetMenu(fileName = ENTITY, menuName = Data.Constants.System.ASSET_PATH + ENTITY, order = 0)]
    public class CharactersConfig : ItemsConfig<CharacterConfig, CharacterData>
    {
        private const string ENTITY = "Characters";
    }
}