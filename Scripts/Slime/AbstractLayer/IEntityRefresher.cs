using System;

namespace Slime.AbstractLayer
{
    public interface IEntityRefresher : IDisposable
    {
        public bool NeedsRefresh { get; }
    }
}