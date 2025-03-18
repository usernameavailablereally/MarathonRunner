using System;
using Core.Services.Events;
using Game.Events;
using UnityEngine.InputSystem;
using VContainer;

namespace Game.Services
{
    public class GameInputService : IDisposable, IGameInput
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly InputAction _leftButtonOrTouchAction;
        private readonly InputAction _restartAction;
        private readonly InputAction _gameOverAction;
        
        [Inject]
        public GameInputService(InputActionAsset inputActionAsset, IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;

            _leftButtonOrTouchAction = inputActionAsset.FindAction(StringConstants.JUMP_INPUT_NAME);
            _leftButtonOrTouchAction.performed += OnJumpPerformed; 
            
            // This is for showcasing the game lifecycle behaviour
            _restartAction = inputActionAsset.FindAction(StringConstants.RESTART_INPUT_NAME);
            _restartAction.performed += ctx => _dispatcherService.Dispatch(new RestartEntryPointEvent());
           
            _gameOverAction = inputActionAsset.FindAction(StringConstants.GAME_OVER_INPUT_NAME);
            _gameOverAction.performed += ctx => _dispatcherService.Dispatch(new ObstacleHitEvent());
        }

        public void Enable()
        {
            _leftButtonOrTouchAction.Enable();
            _restartAction.Enable();
            _gameOverAction.Enable();
        }

        public void Disable()
        {
            _leftButtonOrTouchAction.Disable();
            _restartAction.Disable();
            _gameOverAction.Disable();
            
        }

        private void OnJumpPerformed(InputAction.CallbackContext obj)
        {
            _dispatcherService.Dispatch(new PlayerJumpEvent());
        }

        public void Dispose()
        {
            _leftButtonOrTouchAction?.Dispose();
            _restartAction?.Dispose();
            _gameOverAction?.Dispose();
            Disable();
        }
    }

    public interface IGameInput
    {
        void Enable();
        void Disable();
    }
}