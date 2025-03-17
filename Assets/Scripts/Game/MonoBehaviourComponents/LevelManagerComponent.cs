using System;
using System.Collections.Generic;
using Core.Services.Events;
using Game.Configs;
using Game.Events;
using Game.MonoBehaviourComponents.LoadingAssets;
using UnityEngine;
using VContainer;

namespace Game.MonoBehaviourComponents
{
    /// <summary>
    /// Managing and monitoring scene components
    /// </summary>
    public class LevelManagerComponent : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _runnerSpawnPoint;
        [SerializeField] private BackgroundRunnerComponent _backgroundRunnerComponent;
        [SerializeField] private RoundTimerComponent _roundTimer;
        [SerializeField] private CameraControllerComponent _cameraControllerComponent;
        private IDispatcherService _dispatcherService;
        private MatchConfig _matchConfig;

        [Inject]
        public void Construct(MatchConfig matchConfig, IDispatcherService dispatcherService)
        {
            _matchConfig = matchConfig;
            _dispatcherService = dispatcherService;
        }

        public void InitEvents()
        {
            _dispatcherService.Subscribe<RoundStartEvent>(OnRoundStart);
        }

        private void OnRoundStart(RoundStartEvent obj)
        {
            _backgroundRunnerComponent.Activate(_matchConfig.MovingVelocity);
            _roundTimer.Activate();
        }

        public void InitEnvironment(List<ColumnComponent> columns)
        {
            _cameraControllerComponent.ResetCameraPosition();
            
            var columnPositioner = new ColumnPositioner();
            columnPositioner.Init(columns);
            Vector3 maxRightColumnPosition = columnPositioner.GetLastColumnPosition();
            Vector3 minLeftColumnPosition = columnPositioner.GetFirstColumnPosition();
            _cameraControllerComponent.PlacePointsPositions(minLeftColumnPosition, maxRightColumnPosition);
            
            var obstaclesPositioner = new ObstaclesPositioner();
            obstaclesPositioner.Init(_dispatcherService, _cameraControllerComponent.GetObstacleSpawnPoint());
            _cameraControllerComponent.Activate(columnPositioner, obstaclesPositioner, _matchConfig);
        } 

        public void InitRunner(RunnerControllerComponent runner)
        {
            runner.SetParent(_cameraControllerComponent.transform);
            runner.SetPosition(_runnerSpawnPoint.position);
            runner.Activate();
        }

        public void Dispose()
        {
            _dispatcherService.Unsubscribe<RoundStartEvent>(OnRoundStart);
            _cameraControllerComponent.Deactivate();
            _backgroundRunnerComponent.Deactivate();
            _roundTimer.Deactivate();
        }
    }
}