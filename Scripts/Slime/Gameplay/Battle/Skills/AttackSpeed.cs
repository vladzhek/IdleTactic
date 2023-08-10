using Slime.AbstractLayer;
using Slime.AbstractLayer.Models;
using Slime.AbstractLayer.Stats;
using Slime.Data.Enums;
using Slime.Data.IDs;
using Slime.Gameplay.Battle.Skills.Abstract;

namespace Slime.Gameplay.Battle.Skills
{
    // NOTE: refactor to work with any attribute?
    public class AttackSpeed : Skill
    {
        private const int ACTION_TIME = 5;
        
        private readonly IPlayer _player;
        private readonly ISkillsModel _skillsModel;
        private float _duration;
        private bool _isActive;

        public override string ID { get; set; }

        public override float SecondsToCooldown { get; set; } = 12f;

        public AttackSpeed(IPlayer player, ISkillsModel skillsModel)
        {
            _player = player;
            _skillsModel = skillsModel;
        }
        
        public override void Activate()
        {
            base.Activate();

            SecondsToCooldown = _skillsModel.Get(ID).Cooldown;

            _player.Unit.GetAttribute(AttributeIDs.AttackSpeed)
                .AddModifier(new Modifier(ID, _skillsModel.Get(ID).ActiveValue, EModificationType.Add));
            _isActive = true;
            _duration = 0;
        }

        private void Deactivate()
        {
            _player.Unit.GetAttribute(AttributeIDs.AttackSpeed).RemoveModifier(ID);
            _isActive = false;
        }

        public override void Update(float deltaTime)
        {
            if (_isActive)
            {
                _duration += deltaTime;
                
                if (_duration > ACTION_TIME)
                {
                    _duration = 0;
                    
                    Deactivate();
                }
                
                return;
            }
            
            base.Update(deltaTime);
        }
    }
}