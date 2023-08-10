using Slime.AbstractLayer.Equipment;
using Slime.Data.Attributes;

namespace Slime.AbstractLayer.Models
{
    public interface IAttributesModel : 
        IDisplayableModel<AttributeData>, 
        IUpgradableModel<AttributeData>
    {
    }
}