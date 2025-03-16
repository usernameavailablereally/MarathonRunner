using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Services.Factories;
using Core.Services.Factories.Pools;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.MonoBehaviourComponents;
using NUnit.Framework;

namespace Game.Services
{
    public class ObstaclesFactory : AssetsFactoryBase 
    {
        // could be any custom component instead of GameObject
        private ObjectPool<ObstacleComponent> _obstaclesPool;
        private int _spawnedObstaclesCount;
        public async UniTask Init(MatchConfig matchConfig, CancellationToken cancellationToken)
        {
            ValidateMatchConfigAsserts(matchConfig);
            try
            {
                var obstacles = new List<ObstacleComponent>();
                var weightedObstacles = new List<ObstacleData>();

                // Заполнение списка weightedObstacles в соответствии с пропорцией
                foreach (ObstacleData obstacleData in matchConfig.ObstacleDatas)
                {
                    float targetCount = matchConfig.AmountObstaclesInPool * ((float)obstacleData.ProportionInPool / 100);
                    for (var i = 0; i < targetCount; i++)
                    {
                        var obstacle = await LoadPrefab<ObstacleComponent>(obstacleData.ObstaclePrefab, cancellationToken);
                        obstacles.Add(obstacle);
                    }
                }
                // }
                // List<ObstacleComponent> obstacles = new List<ObstacleComponent>();
                // for (int i = 0; i < matchConfig.AmountObstaclesInPool; i++)
                // {
                //     
                // }
                // foreach (ObstacleData obstacleData in matchConfig.ObstacleDatas)
                // {
                //     var obstacle = await LoadPrefab<ObstacleComponent>(obstacleData.ObstaclePrefab, cancellationToken);
                //     obstacles.Add(obstacle);
                // } 
                 _obstaclesPool = new ObjectPool<ObstacleComponent>(obstacles); 
            }
            catch
            {
                Clear();
                throw;
            }
        }

        public ObstacleComponent GetRandomObstacle()
        {
            ObstacleComponent item = _obstaclesPool.GetRandomNext();
            item.Activate();
            _spawnedObstaclesCount++;
            return item;
        }

        public void Return(ObstacleComponent item)
        {
            if (item == null) return;

            // I prefer pools not to be responsible for disabling objects
            item.Deactivate();
            _obstaclesPool.ReturnToPool(item);
            _spawnedObstaclesCount--;
        }

        public override void Clear()
        {
            // Addressables handle dispose is in base 
            base.Clear();
 
            _obstaclesPool?.Dispose();
            _obstaclesPool = null;
        }

        private void ValidateMatchConfigAsserts(MatchConfig config)
        {
            Assert.IsNotNull(config, "MatchConfig is null");
            Assert.IsTrue(config.ObstacleDatas.Length > 0, "Obstacles array is empty");
            int proportionSum = config.ObstacleDatas.Sum(obstacle => obstacle.ProportionInPool);
            Assert.IsTrue(proportionSum == 100, "Sum of proportions should be 100");
        }
    }
}