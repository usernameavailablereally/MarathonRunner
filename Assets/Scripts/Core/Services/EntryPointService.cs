using System;
using System.Threading;
using Core.Services.Events;
using Core.Services.Match;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Services
{
    /// <summary>
    /// Concept of EntryPoint -> Services (Loading, Lobby, Match) -> Match.Rounds (Logics)
    /// Rounds could be a collection of many, if required
    /// There could be a LobbyService before MatchService, if many Scenes or Screens are intended
    /// </summary>
    public class EntryPointService : IAsyncStartable, IDisposable
    {
        private readonly IMatchService _matchService;
        private readonly IDispatcherService _dispatcherService;
        private CancellationTokenSource _gameStartCancellationTokenSource;
        private bool _isRestarting;
        private bool _disposed;

        [Inject]
        public EntryPointService(IMatchService matchService, IDispatcherService dispatcherService)
        { 
            _matchService = matchService ?? throw new ArgumentNullException(nameof(matchService));
            _dispatcherService = dispatcherService ?? throw new ArgumentNullException(nameof(dispatcherService));
            _dispatcherService.Subscribe<RestartEntryPointEvent>(OnRestartTriggered);
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            Application.targetFrameRate = 60;
            if (_disposed) throw new ObjectDisposedException(nameof(EntryPointService));
        
            _gameStartCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellation);
        
            await _matchService.BuildScene(_gameStartCancellationTokenSource.Token);
            await _matchService.RunGame();
        }

        private void OnRestartTriggered(RestartEntryPointEvent data)
        {
            if (_isRestarting || _disposed) return;
            Debug.Log("<color=white>Restarting game...</color>");
            ReStartAsync().Forget();
        }

        /// <summary>
        /// Restarting the EntryPoint.
        /// No crucial need to have it, just showing how lifecycle is clean and idempotent 
        /// </summary>
        private async UniTask ReStartAsync()
        {
            try
            {
                _isRestarting = true;
                _gameStartCancellationTokenSource?.Cancel();
                _gameStartCancellationTokenSource?.Dispose();
 
                _matchService.Dispose();

                _gameStartCancellationTokenSource = new CancellationTokenSource();
                await StartAsync(_gameStartCancellationTokenSource.Token);
            }
            catch (Exception)
            {
                Debug.Log("<color=red>Failed to restart game</color>");
                _isRestarting = false;  
                throw;
            }
            finally
            {
                _isRestarting = false;  
            } 
        }

        public void Dispose()
        {
            if (_disposed) return;
            
            _disposed = true;
            _isRestarting = false;
            _gameStartCancellationTokenSource?.Cancel();
            _gameStartCancellationTokenSource?.Dispose();
            _gameStartCancellationTokenSource = null;
            _dispatcherService.Unsubscribe<RestartEntryPointEvent>(OnRestartTriggered);
            _dispatcherService.ClearAllSubscriptions();
        }
    }
}