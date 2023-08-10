using System;
using Slime.Data.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Slime.UI.Common
{
    public class PlacementWidget : MonoBehaviour
    {
        public event Action<int> OnPlacementClick; 

        [SerializeField] private GameObject _companion;
        [SerializeField] private GameObject _IsCanEquip;
        [SerializeField] private Image _placement;
        [SerializeField] private Button ClickedArea;

        private int _index;

        private void Start()
        {
            ClickedArea.onClick.AddListener(OnAreaClicked);
        }

        public void SetCompanion(Sprite sprite, ERarity rarity, int index)
        {
            _companion.SetActive(true);
            _companion.GetComponent<Image>().sprite = sprite;
            _placement.color = rarity.ToColor();
            _index = index;
        }

        public void SetActive(bool isActive)
        {
            _companion.SetActive(isActive);
            _IsCanEquip.SetActive(false);
            _placement.color = Color.white;
        }

        public void ActiveSlot(bool isActive)
        {
            _IsCanEquip.SetActive(isActive);
        }

        private void OnAreaClicked()
        {
            OnPlacementClick?.Invoke(_index);
        }
    }
}