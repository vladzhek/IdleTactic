using Slime.Data.Skills;
using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Battle
{
    public class SkillPanelElement : LayoutElement<SkillPanelElement,SkillPanelViewData>
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _cooldownImage;
        [SerializeField] private GameObject _notOpenedSkill;
        [SerializeField] private GameObject _availableForEquipped;
        [SerializeField] private GameObject _freeSlot;

        public override void SetData(SkillPanelViewData data)
        {
            base.SetData(data);

            _image.sprite = data.Sprite;
            _notOpenedSkill.SetActive(!data.IsOpened);
            _image.gameObject.SetActive(data.IsEquipped);
            _availableForEquipped.SetActive(data.IsOpened && data.IsCanEquipped);
            _freeSlot.SetActive(!data.IsEquipped && data.IsOpened);

            _cooldownImage.gameObject.SetActive(data.IsEquipped);
            _cooldownImage.fillAmount = 1 - data.Cooldown;
        }
    }
}