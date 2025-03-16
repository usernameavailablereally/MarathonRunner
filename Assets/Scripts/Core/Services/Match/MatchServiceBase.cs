using System;
using System.Threading;
using Core.Services.Events;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Services.Match
{
    public abstract class MatchServiceBase : IMatchService, IDisposable
    {
        private readonly IDispatcherService _dispatcherService;

        protected CancellationTokenSource RoundTokenSource;

        protected MatchServiceBase(IDispatcherService dispatcherService)
        { 
            _dispatcherService = dispatcherService; 
        }

        public virtual UniTask BuildScene(CancellationToken buildCancellationToken)
        {
            _dispatcherService.Subscribe<RoundOverEvent>(OnRoundOver);
            return UniTask.CompletedTask;
        }

        public async UniTask RunGame()
        {
            try
            {
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
             
            StartRoundLogic();
            return UniTask.CompletedTask;
        }

        protected abstract void StartRoundLogic();

        protected abstract void DisposeRound();

        private void OnRoundOver(RoundOverEvent obj)
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
            _dispatcherService.Unsubscribe<RoundOverEvent>(OnRoundOver);
        }
    }
}