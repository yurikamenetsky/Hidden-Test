using UnityEngine;
using VContainer.Unity;

namespace Services
{
    public class InputService : IInputService, ITickable
    {
        private bool _isEnabled;
        
        public void Initialize()
        {
            _isEnabled = false;
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void Tick()
        {
            if (!_isEnabled) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
