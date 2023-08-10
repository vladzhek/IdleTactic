using System;
using ModestTree.Util;
using Slime.Data.Enums;
using Slime.UI.Common;
using TMPro;
using UnityEngine;

namespace Slime.UI.Footer.Dungeons
{
    public class DungeonLayoutElement : MonoBehaviour
    {
        public event Action<EStageType> LoadDungeon;

        [SerializeField] private EStageType _stageType;

        [SerializeField] private GenericButton _bossRushButton;
        [SerializeField] private GameObject _adsGameObject;

        [SerializeField] private TextMeshProUGUI _bossRushText;
        [SerializeField] private TextMeshProUGUI _bossRushTimeToNewKeyText;
        [SerializeField] private TextMeshProUGUI _bossRushLevelText;
        [SerializeField] private TextMeshProUGUI _bossRushRewardText;
        [SerializeField] private TextMeshProUGUI _bossRushAdsValueText;

        private void FixedUpdate()
        {
            SetTimeToNewKeys();
        }

        private void OnEnable()
        {
            _bossRushButton.onClick.AddListener(StartDungeon);
        }

        private void OnDisable()
        {
            _bossRushButton.onClick.RemoveListener(StartDungeon);
        }

        private void StartDungeon()
        {
            LoadDungeon?.Invoke(_stageType);
        }

        public void UpdateButton(float value, Sprite icon, bool isAdsAvailable)
        {
            _bossRushButton.IconSprite = icon;

            _adsGameObject.SetActive(value == 0 && isAdsAvailable);

            if (value == 0)
            {
                _bossRushButton.interactable = isAdsAvailable;
                
                if (isAdsAvailable)
                {
                    _bossRushText.text = "Watch ads";
                }
            }
        }

        public void SetResourceValue((int value, bool isAds) value)
        {
            if (value.isAds && value.value > 0)
            {
                _bossRushAdsValueText.text = $"{value.Item1}/{2}";
            }
            else
            {
                _bossRushText.text = $"{value.Item1}/{2}";
            }
        }

        public void SetTimeToNewKeys()
        {
            // TODO: hardcode
            _bossRushTimeToNewKeyText.text = $"New key in: <#FFBC57> {DungeonsViewModel.GetTimeToNewKeys()} </color>";
        }

        public void SetDungeonRewards(string value)
        {
            _bossRushRewardText.text = value;
        }

        public void SetDungeonLevel(string value)
        {
            _bossRushLevelText.text = value;
        }
    }
}