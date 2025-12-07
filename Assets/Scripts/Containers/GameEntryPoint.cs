using System;
using Presenters;
using Services;
using VContainer.Unity;

namespace Containers
{
    public class GameEntryPoint : IStartable
    {
        private readonly ILevelPresenter _levelPresenter;
        private readonly IUIService _uiService;
        private readonly IInputService _inputService;

        public GameEntryPoint(
            ILevelPresenter levelPresenter,
            IUIService uiService,
            IInputService inputService)
        {
            _levelPresenter = levelPresenter;
            _uiService = uiService;
            _inputService = inputService;
        }

        public void Start()
        {
            _levelPresenter.Initialize();
            _uiService.Initialize();
            _inputService.Initialize();
        
            // Start game
            _levelPresenter.StartGame();
        }
    }
}
