using System;
using Slime.AbstractLayer.Equipment;
using Slime.Data.Characters;

namespace Slime.AbstractLayer.Models
{
    public interface ICharacterModel : 
        IDisplayableModel<CharacterData>, 
        IUpgradableModel<CharacterData>
    {
        public event Action OnEquip;
        
        public IUnitAvatar GetAvatar(string ID);

        public CharacterData GetEquipped();

        public bool IsEquipped(string ID);
        
        public void Equip(string ID);
    }
}