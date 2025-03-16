using System;
using System.Collections.Generic;
using Core.Services.Events;
using Game.Events;
using UnityEngine;
using VContainer;

namespace Game.MonoBehaviourComponents
{
    /// <summary>
    /// Managing and monitoring positions of components
    /// </summary>
    public class LevelManagerComponent : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _movingVelocity = 2;
        [SerializeField] private Transform _runnerSpawnPoint;
        [SerializeField] private BackgroundRunnerComponent _backgroundRunnerComponent;

        // Could be Transforms on scene for better visualization
        private readonly Vector3 _columnsSpawnPoint = new(10f, COLUMNS_POSITION_Y, 0);
        private readonly Vector2 _obstaclesSpawnPoint = new(15, -2);
        private readonly Vector3 _movementDragVector = new(-1, 0, 0);
       
        private const float FINALIZATION_POSITION_X = -10;
        private const float COLUMNS_POSITION_Y = -3;
        private List<ColumnComponent> _columns;
        private List<ObstacleComponent> _currentObstacles;
        private IDispatcherService _dispatcherService;
        private bool _isGameRunning;
       

        [Inject]
        public void Construct(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
        }

        public void InitEvents()
        {
            _dispatcherService.Subscribe<RoundStartEvent>(OnRoundStart);
            _dispatcherService.Subscribe<RoundPauseStopEvent>(OnRoundPause);
            _dispatcherService.Subscribe<SpawnObstacleRequested>(OnSpawnObstacleRequested);
        }

        private void OnRoundStart(RoundStartEvent obj)
        {
            _isGameRunning = true;
            _backgroundRunnerComponent.Run();
        }

        private void OnRoundPause(RoundPauseStopEvent obj)
        {
            _isGameRunning = false;
            _backgroundRunnerComponent.Stop();
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

            Vector3 movementDrag = _movementDragVector * (_movingVelocity * Time.deltaTime);
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
            _dispatcherService.Unsubscribe<RoundPauseStopEvent>(OnRoundPause);
            _dispatcherService.Unsubscribe<SpawnObstacleRequested>(OnSpawnObstacleRequested);
            _currentObstacles.Clear();
            _columns.Clear();
            _backgroundRunnerComponent.Dispose();
            _isGameRunning = false;
        } 
    }
}