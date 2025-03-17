using Core.Services.Events;
using Game.Events;
using Game.MonoBehaviourComponents.LoadingAssets;
using UnityEngine;

namespace Game.MonoBehaviourComponents
{
    public interface IObstacleHitHandler
    {
        void HandleObstacleHit(ObstacleComponent finalizedObstacle);
        void Dispose();
    }
    
    public class ObstaclesPositioner : IObstacleHitHandler
    {
        private Transform _obstaclesSpawnPoint;
        private IDispatcherService _dispatcherService;

        public void Init(IDispatcherService dispatcherService, Transform obstaclesSpawnPoint)
        {
            _dispatcherService = dispatcherService;
            _obstaclesSpawnPoint = obstaclesSpawnPoint;
            _dispatcherService.Subscribe<SpawnObstacleRequested>(OnSpawnObstacleRequested);
        }

        public void HandleObstacleHit(ObstacleComponent finalizedObstacle)
        {
            _dispatcherService.Dispatch(new ObstacleFinishedEvent(finalizedObstacle));
        }

        private void OnSpawnObstacleRequested(SpawnObstacleRequested data)
        {
            InitNextObstacle(data.SpawnedObstacle);
        }

        private void InitNextObstacle(ObstacleComponent obstacle)
        { 
            obstacle.SetPosition(_obstaclesSpawnPoint.position);
            obstacle.SetRotation(Quaternion.identity);
        }

        public void Dispose()
        {
            _dispatcherService.Unsubscribe<SpawnObstacleRequested>(OnSpawnObstacleRequested);
        }
    }

}