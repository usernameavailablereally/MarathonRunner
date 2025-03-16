using System;
using System.Threading;
using Core.Services.Events;
using Core.Services.Loaders;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Services.Match
{
    public abstract class MatchServiceBase : IMatchService, IDisposable
    {
        private readonly IDispatcherService _dispatcherService;

        protected CancellationTokenSource _roundTokenSource;
        private IMatchData _matchData;

        protected MatchServiceBase(IDispatcherService dispatcherService)
        { 
            _dispatcherService = dispatcherService; 
        }

        public virtual UniTask BuildScene(IAssetsLoader assetsLoader, CancellationToken buildCancellationToken)
        {
            _dispatcherService.Subscribe<GameOverEvent>(OnGameOverEvent);
            return UniTask.CompletedTask;
        } 

        public async UniTask RunGame()
        {
            try
            {
                _matchData = new MatchDataBase();
                await StartRound();
            }
            catch (OperationCanceledException)
            {
                Dispose();
            }
        }

        private UniTask StartRound()
        {
            // Round token linked to round-items animations
            _roundTokenSource?.Cancel();
            _roundTokenSource = new CancellationTokenSource();

            _matchData.UpdateRound();
            if (IsGameFinished())
            {
                _dispatcherService.Dispatch(new RestartGameEvent());
                return UniTask.CompletedTask;
            }
             
            StartRoundLogic();
            return UniTask.CompletedTask;
        }

        protected abstract void StartRoundLogic();

        protected abstract void EndRound();

        private bool IsGameFinished()
        {
            return false;
        }

        private async void OnGameOverEvent(GameOverEvent obj)
        {
            try
            {
                EndRound();
                await StartRound();
            }
            catch (Exception e)
            {
               Debug.LogError(e);
            }
        }

        public void Dispose()
        {
            _dispatcherService.Unsubscribe<GameOverEvent>(OnGameOverEvent);
        }
    }
}