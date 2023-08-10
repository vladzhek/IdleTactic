using System.Collections.Generic;
using System.Linq;
using Reactive;
using Services;
using Slime.AbstractLayer;
using Slime.AbstractLayer.Battle;
using Slime.AbstractLayer.Models;
using Slime.Factories;
using Slime.Models.Abstract;
using Utils.UpdateLoops;

namespace Slime.Models
{
    public class SkillsManager : 
        BaseProgressModel, 
        ISkillsManager, 
        IUpdateInLoop
    {
        private const string ACTIVE_AUTO_SKILL = "isAutoSkillActive";
        private const int MAX_SKILLS_COUNT = 6;

        private readonly ISkillFactory _skillFactory;
        private readonly ISkillsModel _skillsModel;
        private readonly ISettingsModel _settingModel;
        private readonly IPlayer _player;

        private UpdateLoopsService _updateLoopsService;
        private bool _isAutoSkillActive;

        public int MaxSkillsCount => MAX_SKILLS_COUNT;
        public bool IsAutoSkillActive { get; private set; }

        public Dictionary<string, ISkill> Skills { get; } = new();

        public EUpdateLoop UpdateLoop => EUpdateLoop.Skills;

        public bool IsActive { get; } = true;

        public SkillsManager(IGameProgressModel progressModel, UpdateLoopsService updateLoopsService,
            ISkillFactory skillFactory, 
            ISkillsModel skillsModel,
            ISettingsModel settingModel,
            IPlayer player) : base(progressModel)
        {
            updateLoopsService.RegisterForUpdate(this);
            _skillFactory = skillFactory;
            _skillsModel = skillsModel;
            _settingModel = settingModel;
            _player = player;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            _skillsModel.OnChange += OnSkillsChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            
            _skillsModel.OnChange -= OnSkillsChanged;
        }

        protected override void OnProgressLoaded()
        {
            UpdateState();
        }
        
        private void OnSkillsChanged()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            // equipped skills
            var equippedData = GameData.EquippedSkillsData;
            Skills.Clear();

            for (var i = 0; i < MAX_SKILLS_COUNT; i++)
            {
                if (equippedData.TryGetValue(i, out var equippedSkillData))
                {
                    Skills[equippedSkillData] = _skillFactory.CreateSkill(equippedSkillData);
                }
            }
            
            // auto skill
            IsAutoSkillActive = _settingModel.GetBool(ACTIVE_AUTO_SKILL);
        }
        
        public void Update(float deltaTime)
        {
            var skills = Skills.Values;
            foreach (var skill in skills)
            {
                if (skill == null)
                {
                    continue;
                }

                skill.Update(deltaTime);

                if (!skill.IsReady) continue;
                if (!IsAutoSkillActive) continue;
                if (_player.Unit.Status != ".AttackState") continue;

                skill.Activate();
            }
        }

        public void SetAutoSkill(bool isAutoSkill)
        {
            IsAutoSkillActive = isAutoSkill;
            _settingModel.Set(ACTIVE_AUTO_SKILL, isAutoSkill);
        }

        public void ActivateSkill(string id)
        {
            if (!string.IsNullOrEmpty(id) && Skills.TryGetValue(id, out var skill))
            {
                if (skill.IsReady)
                {
                    skill.Activate();
                }
            }
        }
    }
}