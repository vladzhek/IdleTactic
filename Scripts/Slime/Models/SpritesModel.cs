using System.Collections.Generic;
using Slime.AbstractLayer.Models;
using Slime.Data;
using Slime.Data.IDs;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.Models
{
    public class SpritesModel : ISpritesModel
    {
        private readonly Dictionary<string, Sprite> _sprites = new();

        public void SetSpritesCollection(IEnumerable<SpriteEntry> spriteEntries)
        {
            foreach (var spriteEntry in spriteEntries)
            {
                if (_sprites.ContainsKey(spriteEntry.ID))
                {
                    Logger.Warning($"Sprite |{spriteEntry.ID}| already was in collection!");
                }

                _sprites[spriteEntry.ID] = spriteEntry.Sprite;
            }
        }

        public Sprite Get(string id)
        {
            return Get(id, true);
        }
        
        public Sprite Get(string id, bool shouldReturnDefault)
        {
            while (true)
            {
                if (string.IsNullOrEmpty(id))
                {
                    Logger.Warning($"attempted to get null sprite");
                    if (!shouldReturnDefault)
                    {
                        return null;
                    }
                    
                    id = SpritesIDs.Default;
                }

                if (_sprites.TryGetValue(id, out var sprite))
                {
                    return sprite;
                }
                
                Logger.Warning($"no sprite with id {id}");
                if (!shouldReturnDefault || id == SpritesIDs.Default)
                {
                    return null;
                }
                
                id = SpritesIDs.Default;
            }
        }
    }
}