using System;
using System.Collections.Generic;
using Reactive;
using Slime.AbstractLayer.Battle;
using Zenject;

namespace Slime.AbstractLayer
{
    public interface ISkillsManager : IInitializable, IDisposable
    {
        int MaxSkillsCount { get; }
        bool IsAutoSkillActive { get; }
        Dictionary<string, ISkill> Skills { get; }
        void SetAutoSkill(bool isAutoSkill);
        void ActivateSkill(string id);
    }
}