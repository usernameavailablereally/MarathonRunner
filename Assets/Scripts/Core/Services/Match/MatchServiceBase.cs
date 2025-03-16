using System;
using System.Threading;
using Core.Services.Events;
using Core.Services.Loaders;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Services.Match
{
    public abstract class MatchServiceBase : IMatchService, IDisposable
    {
        private readonly IDispatcherService _dispatcherService;

        protected CancellationTokenSource RoundTokenSource;
        private IMatchData _matchData;

        protected MatchServiceBase(IDispatcherService dispatcherService)
        { 
            _dispatcherService = dispatcherService; 
        }

        public virtual UniTask BuildScene(IAssetsLoader assetsLoader, CancellationToken buildCancellationToken)
        {
            _dispatcherService.Subscribe<RoundOverEvent>(OnGameOverEvent);
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
            RoundTokenSource?.Cancel();
            RoundTokenSource = new CancellationTokenSource();

            _matchData.UpdateRound();
            if (IsGameFinished())
            {
                _dispatcherService.Dispatch(new RestartEntryPointEvent());
                return UniTask.CompletedTask;
            }
             
            StartRoundLogic();
            return UniTask.CompletedTask;
        }

        protected abstract void StartRoundLogic();

        protected abstract void DisposeRound();

        private bool IsGameFinished()
        {
            return false;
        }

        private void OnGameOverEvent(RoundOverEvent obj)
        {
            try
            {
                DisposeRound();
            }
            catch (Exception e)
            {
               Debug.LogError(e);
            }
        }

        public void Dispose()
        {
            DisposeRound();
            _dispatcherService.Unsubscribe<RoundOverEvent>(OnGameOverEvent);
        }
    }
}