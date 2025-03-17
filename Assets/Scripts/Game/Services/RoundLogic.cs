using System.Threading;
using Core.Services.Events;
using Core.Services.Match;
using Game.Configs;
using Game.Events;
using Game.MonoBehaviourComponents.Objects;
using UnityEngine;

namespace Game.Services
{
    public class RoundLogic : IRoundLogic
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly ObstaclesFactory _obstaclesFactory;

        // Needed for linking round-related Tasks (e.g. animations, delays)
        private CancellationToken _roundCancellationToken;
        private bool _isRoundRunning;
        private float _timeSinceLastSpawn;
        readonly MatchConfig _matchConfig;
        // this value is independent of the one calculated in UI RoundTimer
        private float _roundStartTime;

        public RoundLogic(MatchConfig matchConfig, IDispatcherService dispatcherService, ObstaclesFactory obstaclesFactory)
        {
            _matchConfig = matchConfig;
            _dispatcherService = dispatcherService;
            _obstaclesFactory = obstaclesFactory;
        }

        public void StartRound(CancellationToken roundCancellationToken)
        {
            _roundCancellationToken = roundCancellationToken;
            _timeSinceLastSpawn = 0f;
            _isRoundRunning = true;
            _roundStartTime = Time.time;
            _dispatcherService.Dispatch(new RoundStartEvent());

            _dispatcherService.Subscribe<ObstacleFinishedEvent>(OnObstacleFinished);
            _dispatcherService.Subscribe<ObstacleHitEvent>(OnObstacleHit);
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

            if (!CanSpawnObstacleByTime(_timeSinceLastSpawn)) return;

            _timeSinceLastSpawn = 0f;

            if (CanSpawnObstacleByLimit() && CanSpawnObstacleByThreshold())
            {
                SpawnObstacle();
            }
        }

        private bool CanSpawnObstacleByTime(float timeSinceLastSpawn)
        {
            return timeSinceLastSpawn >= _matchConfig.SpawnObstacleInterval;
        }

        private bool CanSpawnObstacleByThreshold()
        {
            float randomChance = Random.Range(0f, 1f);  
            return randomChance <= _matchConfig.SpawnObstacleThreshold;
        }

        private bool CanSpawnObstacleByLimit()
        {
            return _obstaclesFactory.SpawnedObstaclesCount < _matchConfig.MaxObstaclesOnScene;
        }

        private void SpawnObstacle()
        {
            ObstacleComponent randomObstacle = _obstaclesFactory.GetRandomObstacle();
            _dispatcherService.Dispatch(new SpawnObstacleRequested(randomObstacle));
        }

        private void OnObstacleFinished(ObstacleFinishedEvent data)
        {
            _obstaclesFactory.Return(data.Obstacle);
        }

        private void OnObstacleHit(ObstacleHitEvent obj)
        {
            float roundDuration = Time.time - _roundStartTime;
            _dispatcherService.Dispatch(new RoundOverEvent(roundDuration));
        }
    }
}