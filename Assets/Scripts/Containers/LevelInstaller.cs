using Data;
using Presenters;
using Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Views;

namespace Containers
{
    public class LevelInstaller : LifetimeScope
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private UIService uiService;
        [SerializeField] private LevelView levelView;
    
        private InputService _inputService;
        
        private LevelPresenter _levelPresenter;

        protected override void Configure(IContainerBuilder builder)
        {
            // register LevelData ScriptableObject
            builder.RegisterInstance(levelData);
            builder.RegisterInstance<ILevelView, LevelView>(levelView);
        
            // Register UI Service as component
            builder.RegisterComponent(uiService).As<IUIService>();
            
            // Register services as Singletons
            builder.Register<IInputService, InputService>(Lifetime.Singleton).As<ITickable>();
            
            // Register LevelPresenter
            builder.Register<ILevelPresenter, LevelPresenter>(Lifetime.Singleton).As<ITickable>();

            // Create and register EntryPoint
            builder.RegisterEntryPoint<GameEntryPoint>();
        }
    }
}
