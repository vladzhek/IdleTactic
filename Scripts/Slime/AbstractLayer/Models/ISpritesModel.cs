using System.Collections.Generic;
using Slime.Data;
using UnityEngine;

namespace Slime.AbstractLayer.Models
{
    public interface ISpritesModel
    {
        // TODO: redo with config
        public void SetSpritesCollection(IEnumerable<SpriteEntry> spriteEntries);
        public Sprite Get(string id);
        public Sprite Get(string id, bool shouldReturnDefault);
    }
}