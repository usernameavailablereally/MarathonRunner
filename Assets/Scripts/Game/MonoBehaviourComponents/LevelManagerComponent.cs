using System;
using System.Collections.Generic;
using Core.Services.Events;
using Game.Configs;
using Game.Events;
using Game.MonoBehaviourComponents.Objects;
using UnityEngine;
using VContainer;

namespace Game.MonoBehaviourComponents
{
    /// <summary>
    /// Managing and monitoring positions of components
    /// </summary>
    public class LevelManagerComponent : MonoBehaviour, IDisposable
    {
        [SerializeField] private Transform _runnerSpawnPoint;
        [SerializeField] private BackgroundRunnerComponent _backgroundRunnerComponent;
        [SerializeField] private RoundTimerComponent _roundTimer;

        // Could be Transforms on scene for better visualization
        private readonly Vector3 _columnsSpawnPoint = new(12f, COLUMNS_POSITION_Y, 0);
        private readonly Vector2 _obstaclesSpawnPoint = new(15, -1.9f);
        private readonly Vector3 _movementDragVector = new(-1, 0, 0);
       
        private const float FINALIZATION_POSITION_X = -12;
        private const float COLUMNS_POSITION_Y = -3;
        private List<ColumnComponent> _columns;
        private List<ObstacleComponent> _currentObstacles;
        private IDispatcherService _dispatcherService;
        private bool _isGameRunning;
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
            _dispatcherService.Subscribe<SpawnObstacleRequested>(OnSpawnObstacleRequested);
        }

        private void OnRoundStart(RoundStartEvent obj)
        {
            _isGameRunning = true;
            _backgroundRunnerComponent.Activate(_matchConfig.MovingVelocity);
            _roundTimer.Activate();
        }

        public void InitColumns(List<ColumnComponent> columns)
        {
            for (var index = 0; index < columns.Count; index++)
            {
                ColumnComponent column = columns[index];
                column.SetPosition(new Vector2(FINALIZATION_POSITION_X + index, COLUMNS_POSITION_Y));
                column.SetRotation(Quaternion.identity);
                column.Activate();
            }

            _columns = columns;
        }

        public void InitObstacles()
        {
            _currentObstacles = new List<ObstacleComponent>();
        }

        public void InitRunner(RunnerControllerComponent runner)
        {
            runner.SetPosition(_runnerSpawnPoint.position);
            runner.Activate();
        }

        private void Update()
        {
            if (!_isGameRunning || _columns == null || _currentObstacles == null)
            {
                return;
            }

            Vector3 movementDrag = _movementDragVector * (_matchConfig.MovingVelocity * Time.deltaTime);
            ProcessObstaclesMovement(movementDrag);
            ProcessColumnMovement(movementDrag);

        }

        private void ProcessColumnMovement(Vector3 movementDrag)
        {
            foreach (ColumnComponent column in _columns)
            {
                if (column.transform.position.x <= FINALIZATION_POSITION_X)
                {
                    column.SetPosition(_columnsSpawnPoint);
                }

                column.TranslatePosition(movementDrag);
            }
        }

        private void ProcessObstaclesMovement(Vector3 movementDrag)
        { 
            for (var i = 0; i < _currentObstacles.Count; i++)
            {
                if (_currentObstacles[i].transform.position.x <= FINALIZATION_POSITION_X)
                {
                    FinalizeObstacle(i);
                    return;
                }

                _currentObstacles[i].TranslatePosition(movementDrag);
            }
        }

        private void FinalizeObstacle(int index)
        {
            _dispatcherService.Dispatch(new ObstacleFinishedEvent(_currentObstacles[index]));
            _currentObstacles.RemoveAt(index);
        }

        private void OnSpawnObstacleRequested(SpawnObstacleRequested data)
        {
            InitNextObstacle(data.SpawnedObstacle);
        }

        private void InitNextObstacle(ObstacleComponent obstacle)
        { 
            obstacle.SetPosition(_obstaclesSpawnPoint);
            obstacle.SetRotation(Quaternion.identity);
            _currentObstacles.Add(obstacle);
        }

        public void Dispose()
        {
            _dispatcherService.Unsubscribe<RoundStartEvent>(OnRoundStart);
            _dispatcherService.Unsubscribe<SpawnObstacleRequested>(OnSpawnObstacleRequested);
            _currentObstacles.Clear();
            _columns.Clear();
            _backgroundRunnerComponent.Deactivate();
            _roundTimer.Deactivate();
            _isGameRunning = false;
        } 
    }
}