using System;
using Slime.Data.Constants;
using Slime.UI.Common;
using TMPro;
using UI.Base.Widgets;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;
using Utils.Extensions;
using Utils.Time;

namespace Slime.UI.Boosters
{
    public class BoosterLayoutElement : LayoutElement<BoosterLayoutElement,BoosterLayoutElementViewData>
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _sliderText;
        [FormerlySerializedAs("_busterSprite"), SerializeField] private Image _boosterSprite;
        [SerializeField] private Image _colorBG;
        [SerializeField] private GameObject _busterGreenArrow;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _busterName;
        [SerializeField] private TMP_Text _busterIncreaseName;
        [SerializeField] private TMP_Text _busterValueBonus;
        [SerializeField] private TMP_Text _time;
        [FormerlySerializedAs("_buttonWatchADS")] [SerializeField] private GenericButton _buttonWatchAds;

        private ITimer _timer;

        private void Start()
        {
            _buttonWatchAds.onClick.AddListener(OnSelected);
        }

        public override void SetData(BoosterLayoutElementViewData data)
        {
            base.SetData(data);
            
            _boosterSprite.sprite = data.BoosterSprite;
            _busterGreenArrow.SetActive(true);
            _busterName.text = data.Title;
            _busterIncreaseName.text = data.Description;
            _busterValueBonus.text = data.Value+"%";
            _levelText.text = Strings.LEVEL.Resolve(data.CurrentLvl);
            _slider.value = (float)data.Amount / data.TotalCounts;
            _sliderText.text = $"{data.Amount}/{data.TotalCounts}";
            _colorBG.color = data.ColorBG;
            _time.text = $"{data.Duration}m";
            _buttonWatchAds.Interactable = !data.IsActive;
        }

        public void SetTimer(ITimer timer)
        {
            _timer = timer;

            timer.OnTick += OnTicked;
            timer.OnComplete += OnTimerCompleted;
        }

        private void OnTimerCompleted(ITimer _)
        {
            _timer.OnTick -= OnTicked;
            _timer.OnComplete -= OnTimerCompleted;
        }

        private void OnTicked(int seconds)
        {
            _time.text = FormatTime.MinutesStringFormat(seconds);
        }
    }
}