using System.Collections.Generic;
using Slime.AbstractLayer;

namespace Slime.AbstractLayer.Configs
{
    public interface IUnitsAvatars
    {
        Dictionary<string, IUnitAvatar> GetUnitsAvatars();
    }
}