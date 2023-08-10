using Slime.AbstractLayer.Models;
using Slime.Configs.Sprites;
using UnityEngine;
using Zenject;

namespace Slime.Feeders
{
    public class SpritesFeeder : MonoBehaviour
    {
        [SerializeField] private SpritesCollection[] _spritesCollections;

        [Inject]
        public void Construct(ISpritesModel spritesModel)
        {
            foreach (var spritesCollection in _spritesCollections)
            {
                spritesModel.SetSpritesCollection(spritesCollection.SpriteEntries);
            }
        }
    }
}   