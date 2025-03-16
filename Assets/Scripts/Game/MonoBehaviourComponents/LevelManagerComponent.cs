using System;
using System.Collections.Generic;
using Core.Services.Events;
using Game.Events;
using UnityEngine;
using VContainer;

namespace Game.MonoBehaviourComponents
{
    public class LevelManagerComponent : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _velocity = 2;
        [SerializeField] private Transform _runnerSpawnPoint;
        [SerializeField] private BackgroundRunnerComponent _backgroundRunnerComponent;

        private List<ColumnComponent> _columns;
        private List<ObstacleComponent> _currentObstacles;
        IDispatcherService _dispatcherService;
        private bool _isGameRunning;

        [Inject]
        public void Construct(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
        }

        public void InitEvents()
        {
            _dispatcherService.Subscribe<GameStartEvent>(OnGameStart);
            _dispatcherService.Subscribe<GameStopEvent>(OnGameStop);
            _dispatcherService.Subscribe<SpawnObstacleRequested>(OnSpawnObstacleRequested);
        }

        private void OnGameStart(GameStartEvent obj)
        {
            _isGameRunning = true;
            _backgroundRunnerComponent.Run();
        }

        private void OnGameStop(GameStopEvent obj)
        {
            _isGameRunning = false;
            _backgroundRunnerComponent.Stop();
        }

        public void InitColumns(List<ColumnComponent> columns)
        {
            for (var index = 0; index < columns.Count; index++)
            {
                ColumnComponent column = columns[index];
                column.SetPosition(new Vector2(-10 + index, -3));
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

            ProcessColumnMovement();

            ProcessObstaclesMovement();
        }

        private void ProcessColumnMovement()
        {
            foreach (ColumnComponent column in _columns)
            {
                if (column.transform.position.x <= -10)
                {
                    column.SetPosition(new Vector3(10f, -3, 0));
                }

                column.TranslatePosition(new Vector3(-1, 0, 0) * (_velocity * Time.deltaTime));
            }
        }

        private void ProcessObstaclesMovement()
        {
            for (var i = 0; i < _currentObstacles.Count; i++)
            {
                if (_currentObstacles[i].transform.position.x <= -10)
                {
                    FinalizeObstacle(i);
                    return;
                }

                _currentObstacles[i].TranslatePosition(new Vector3(-1, 0, 0) * (_velocity * Time.deltaTime));
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
            obstacle.SetPosition(new Vector2(15, -2));
            obstacle.SetRotation(Quaternion.identity);
            _currentObstacles.Add(obstacle);
        }

        public void Dispose()
        {
            _dispatcherService.Unsubscribe<GameStartEvent>(OnGameStart);
            _dispatcherService.Unsubscribe<GameStopEvent>(OnGameStop);
            _dispatcherService.Unsubscribe<SpawnObstacleRequested>(OnSpawnObstacleRequested);
            _currentObstacles.Clear();
            _columns.Clear();
            _backgroundRunnerComponent.Dispose();
            _isGameRunning = false;
        } 
    }
}