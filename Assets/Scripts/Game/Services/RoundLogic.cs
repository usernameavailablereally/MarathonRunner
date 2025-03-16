using System.Threading;
using Core.Services.Events;
using Core.Services.Match;
using Game.Events;
using Game.MonoBehaviourComponents;
using UnityEngine;

namespace Game.Services
{
    public class RoundLogic : IRoundLogic
    {
        private const float SPAWN_THRESHOLD = 0.5f; 
        private const float SPAWN_OBSTACLE_INTERVAL = 1f;
        private readonly IDispatcherService _dispatcherService;
        private readonly ObstaclesFactory _obstaclesFactory;

        // Needed for linking round-related Tasks (e.g. animations, delays)
        private CancellationToken _roundCancellationToken;
        private bool _isRoundRunning;
        private float _timeSinceLastSpawn;

        public RoundLogic(IDispatcherService dispatcherService, ObstaclesFactory obstaclesFactory)
        {
            _dispatcherService = dispatcherService;
            _obstaclesFactory = obstaclesFactory;
        }

        public void StartRound(CancellationToken roundCancellationToken)
        {
            _roundCancellationToken = roundCancellationToken;
            _timeSinceLastSpawn = 0f;
            _isRoundRunning = true;
            _dispatcherService.Dispatch(new RoundStartEvent());
            
            _dispatcherService.Subscribe<ObstacleFinishedEvent>(OnObstacleFinished);
        }

        public void EndRound()
        {
            _isRoundRunning = false;
            _dispatcherService.Unsubscribe<ObstacleFinishedEvent>(OnObstacleFinished);
        }

        public void OnTick()
        {
            if (!_isRoundRunning) return;
            
            _timeSinceLastSpawn += Time.deltaTime;

            if (!(_timeSinceLastSpawn >= SPAWN_OBSTACLE_INTERVAL)) return;
            
            SpawnObstacleIfRandom();
            _timeSinceLastSpawn = 0f;
        }

        private void SpawnObstacleIfRandom()
        {
            float randomChance = Random.Range(0f, 1f);  
            if (randomChance <= SPAWN_THRESHOLD)
            {    
                ObstacleComponent randomObstacle = _obstaclesFactory.GetRandomObstacle();
                _dispatcherService.Dispatch(new SpawnObstacleRequested(randomObstacle));
            }
        }

        private void OnObstacleFinished(ObstacleFinishedEvent data)
        {
            _obstaclesFactory.Return(data.Obstacle);
        }
    }
}