using System;
using UnityEngine;
using System.Collections.Generic;
using Data;
using Services;
using VContainer;
using VContainer.Unity;
using Views;

namespace Presenters
{
    public interface ILevelPresenter : IGameService, IDisposable
    {
        bool IsGameActive { get; }
        void StartGame();
    }
    
    public class LevelPresenter : ILevelPresenter, ITickable
    {
        private readonly IUIService _uiService;
        private readonly IInputService _inputService;

        private readonly LevelData _levelData;
        private readonly ILevelView _levelView;
        private Dictionary<ItemData, bool> _levelModel = new();

        private List<ItemData> _activeItems;
        private List<ItemData> _currentSearchItems = new ();
        
        private float _currentTime;
        private bool _isGameActive;

        public bool IsGameActive => _isGameActive;

        [Inject]
        public LevelPresenter(
            LevelData levelData,
            IUIService uiService,
            IInputService inputService,
            ILevelView levelView)
        {
            _levelData = levelData;
            _uiService = uiService;
            _inputService = inputService;
            _levelView = levelView;
        }

        public void Initialize()
        {
            _activeItems = _levelData.GetActiveItems();
            _currentTime = _levelData.TimerDuration;
            _isGameActive = false;
            
            _uiService.GameStarted += StartGame;
        }
        
        public void Dispose()
        {
            _uiService.GameStarted -= StartGame;
        }

        public void StartGame()
        {
            _isGameActive = true;
            _inputService.Enable();
            _currentTime = _levelData.TimerDuration;

            _levelView.Initialize(OnItemFound, OnItemCollected);

            _levelModel.Clear();
            foreach (var item in _activeItems)
            {
                _levelModel.Add(item, false);
            }

            UpdateSearchItems();
        }

        private void UpdateSearchItems()
        {
            _currentSearchItems.Clear();
            var index = 0;
            foreach (var modelItem in _levelModel)
            {
                if (index >= _levelData.MaxItemsCount)
                    break;
                
                if (!modelItem.Value)
                {
                    _currentSearchItems.Add(modelItem.Key);
                    index++;
                }
            }

            _levelView.UpdateItems(_currentSearchItems);
            _uiService.UpdateItemListUI(_currentSearchItems);
        }

        public void EndGame(bool won)
        {
            _isGameActive = false;
            _inputService.Disable();

            if (won)
            {
                _uiService.ShowWinScreen();
            }
            else
            {
                _uiService.ShowLoseScreen();
            }
            _levelView.Destroy();
        }

        private void OnItemFound(ItemData itemData)
        {
            if (!_isGameActive) 
                return;

            var foundIndex = _currentSearchItems.IndexOf(itemData);
            if (foundIndex < 0)
                return;
            
            _uiService.RemoveItemFromUI(itemData);

            _levelView.Collect(itemData);
            _levelModel[itemData] = true;
                
            UpdateSearchItems();
        }

        private void OnItemCollected(ItemData itemData)
        {
            if (_currentSearchItems.Count > 0 || !_isGameActive)
                return;
            
            EndGame(true);
        }
        
        public void Tick()
        {
            if (!_isGameActive || !_levelData.TimerEnabled) 
                return;

            _currentTime -= Time.deltaTime;
            
            if (_currentTime <= 0)
            {
                _currentTime = 0;
                EndGame(false);
            }
            
            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(_currentTime / 60);
            int seconds = Mathf.FloorToInt(_currentTime % 60);
            _uiService.UpdateTimer($"{minutes:00}:{seconds:00}");
        }
    }
}