using System;
using System.Threading;
using Core.Services.Events;
using Core.Services.Factories;
using Core.Services.Loaders.Configs;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Services.Match
{
    public class MatchService : IMatchService, IDisposable
    {
        private readonly IAssetsLoader _assetsLoader;
        private readonly IItemsFactory _itemsFactory;
        private readonly IDispatcherService _dispatcherService;
        private readonly RoundLogic _roundLogic;
        private CancellationTokenSource _roundTokenSource;
        private IItem _roundItem;
        private MatchConfig _matchConfig;
        private IMatchData _matchData;
        
        public MatchService(IAssetsLoader assetsLoader, 
            IDispatcherService dispatcherService)
        {
            _assetsLoader = assetsLoader;
            _dispatcherService = dispatcherService;
            _roundLogic = new RoundLogic(dispatcherService);
            _itemsFactory = new ItemsFactory();
        }

        public async UniTask BuildScene(CancellationToken buildCancellationToken)
        {
            _matchConfig = await _assetsLoader.LoadAndValidateMatchConfig(buildCancellationToken); 
            await _itemsFactory.Init(_matchConfig, buildCancellationToken);
            _dispatcherService.Subscribe<GameOverEvent>(OnGameOverEvent);
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
            
            _roundItem = _itemsFactory.Get();
            _roundLogic.StartRound(_roundItem, _roundTokenSource.Token);
            return UniTask.CompletedTask;
        }

        private bool IsGameFinished()
        {
            return false;
        }

        private void EndRound()
        {
            _roundLogic.EndRound();
            _itemsFactory.Return(_roundItem);
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
            _itemsFactory.Clear();
        }
    }
}