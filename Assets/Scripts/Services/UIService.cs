using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Data;
using VContainer;
using Views.UI;

namespace Services
{
    public class UIService : MonoBehaviour, IUIService
    {
        public event Action GameStarted;
        
        [SerializeField] private Transform _itemListPanel;
        [SerializeField] private HiddenItemUiTextView _itemTextPrefab;
        [SerializeField] private HiddenItemUiImageView _itemImagePrefab;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private EndLevelPanelView _endLevelPanel;

        private List<IHiddenItemUiView> _currentUIItems = new();
        
        private LevelData _levelData;

        [Inject]
        public void Construct(LevelData levelData)
        {
            _levelData = levelData;
        }
        
        public void Initialize()
        {
            _endLevelPanel.Init(OnRestartButtonClicked, OnExitButtonClicked);
            _endLevelPanel.gameObject.SetActive(false);
            ClearItemList();
        }

        public void UpdateItemListUI(List<ItemData> items)
        {
            foreach (var item in items)
            {
                if (_currentUIItems.Exists(x => x.ItemName == item.name))
                    continue;
                
                IHiddenItemUiView uiElement = Instantiate((_levelData.UseImagesInsteadOfText ? 
                    _itemImagePrefab : _itemTextPrefab), _itemListPanel) as IHiddenItemUiView;
                uiElement?.Init(item);
                _currentUIItems.Add(uiElement);
            }
        }

        public void RemoveItemFromUI(ItemData item)
        {
            var remItem = _currentUIItems.Find(x => x.ItemName == item.name);
            if (remItem == null)
                return;

            _currentUIItems.Remove(remItem);
            remItem.Destroy();
        }

        public void UpdateTimer(string time)
        {
            if (_timerText != null)
            {
                _timerText.text = time;
            }
        }

        public void ShowWinScreen()
        {
            _endLevelPanel.Show(true);
        }

        public void ShowLoseScreen()
        {
            _endLevelPanel.Show(false);
        }

        private void ClearItemList()
        {
            foreach (var item in _currentUIItems)
            {
                if (item != null)
                    item.Destroy(true);
            }

            _currentUIItems.Clear();
        }

        // UI Buttons events
        private void OnRestartButtonClicked()
        {
            _endLevelPanel.gameObject.SetActive(false);
            GameStarted?.Invoke();
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}