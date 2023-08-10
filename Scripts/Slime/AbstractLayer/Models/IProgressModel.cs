using System;
using Cysharp.Threading.Tasks;
using Slime.Data.Abstract;
using Slime.Data.Progress;
using Slime.Data.Progress.Abstract;

namespace Slime.AbstractLayer.Models
{
    public interface IProgressModel<T1, T2> : ISavable 
        where T1 : GenericData
        where T2 : GenericData
    {
        public UniTask<T1> Create();
        public T1 Get();
        
        // NOTE: do not use this (!) events when progress changes (?)
        public void Update(T2 data);
        public event Action<T2> Updated;
    }
}