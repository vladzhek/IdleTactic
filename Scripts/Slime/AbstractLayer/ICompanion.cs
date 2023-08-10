using System.Collections.Generic;
using Slime.AbstractLayer.Battle;

namespace Slime.AbstractLayer
{
    public interface ICompanion
    {
        public Dictionary<int,IUnit> Units { get; set; }
        void AddUnit(int index, IUnit unit);
        void OnUpdate(float delta);
        void CreateCompanions();
    }
}