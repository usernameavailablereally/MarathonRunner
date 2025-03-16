using Core.Services.Events;
using Game.Events;
using UnityEngine;
using VContainer.Unity;

namespace Game.Services
{
    public class GameInputService : ITickable
    {
        private readonly IDispatcherService _dispatcherService; 
        private bool _isWaitingForMouseUp;    
        private bool IsLeftButtonDown() => Input.GetMouseButtonDown(0);
        private bool IsLeftButtonUp() => Input.GetMouseButtonUp(0);
        
        public GameInputService(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
        }

        public void Tick()
        {
            CheckForMouseInput();
            CheckForKeyboardInputTMP();
        }

        /// <summary>
        /// This is for showcasing the game lifecycle behaviour
        /// </summary>
        private void CheckForKeyboardInputTMP()
        {
            // Restarting game
            if (Input.GetKeyDown(KeyCode.R))
            {
                _dispatcherService.Dispatch(new RestartEntryPointEvent());
            }
            // Forcing Game Over
            if (Input.GetKeyDown(KeyCode.O))
            {
                _dispatcherService.Dispatch(new RoundOverEvent());
            }
            // Pausing round
            if (Input.GetKeyDown(KeyCode.P))
            {
                _dispatcherService.Dispatch(new RoundPauseStopEvent());
            }
            
        }

        private void CheckForMouseInput()
        {
            if (_isWaitingForMouseUp)
            {
                if (IsLeftButtonUp())
                {
                    _isWaitingForMouseUp = false;
                }
                return;
            }

            if (IsLeftButtonDown())
            {
                _dispatcherService.Dispatch(new PlayerJumpEvent());
                _isWaitingForMouseUp = true;
            }
        } 
    }
}