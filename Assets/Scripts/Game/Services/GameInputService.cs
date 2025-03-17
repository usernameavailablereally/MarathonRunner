using Core.Services.Events;
using Game.Events;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace Game.Services
{
    public class GameInputService : ITickable
    {
        private readonly IDispatcherService _dispatcherService;
        private bool _isWaitingForMouseUp;
        private readonly InputAction _leftButtonOrTouchAction;
        private readonly InputAction _restartAction;
        private readonly InputAction _gameOverAction;
        
        [Inject]
        public GameInputService(InputActionAsset inputActionAsset, IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;

            _leftButtonOrTouchAction = inputActionAsset.FindAction("Jump");
            _leftButtonOrTouchAction.performed += OnJumpPerformed; 
            
            // This is for showcasing the game lifecycle behaviour
            _restartAction = inputActionAsset.FindAction("Restart");
            _restartAction.performed += ctx => _dispatcherService.Dispatch(new RestartEntryPointEvent());
           
            _gameOverAction = inputActionAsset.FindAction("GameOVer");
            _gameOverAction.performed += ctx => _dispatcherService.Dispatch(new ObstacleHitEvent());

            _leftButtonOrTouchAction.Enable();
            _restartAction.Enable();
            _gameOverAction.Enable();
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            _dispatcherService.Dispatch(new PlayerJumpEvent());
        }

        public void Tick()
        { 
        }
    }
}