using Slime.Data.Enums;

namespace Slime.AbstractLayer.Models
{
    public interface ISummonInfoUIModel
    {
        public ESummonType Type { get; }
        void Open(ESummonType type);
        void Close();
    }
}