using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Services.Factories;
using Core.Services.Factories.Pools;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.MonoBehaviourComponents.LoadingAssets;
using NUnit.Framework;
using UnityEngine;

namespace Game.Services
{
    public class ObstaclesFactory : AssetsFactoryBase 
    {
        public int SpawnedObstaclesCount;
        // could be any custom component instead of GameObject
        private ObjectPool<ObstacleComponent> _obstaclesPool;
        public async UniTask Init(AssetsConfig assetsConfig, CancellationToken cancellationToken)
        {
            ValidateMatchConfigAsserts(assetsConfig);
            try
            {
                var obstacles = new List<ObstacleComponent>();
                foreach (ObstacleData obstacleData in assetsConfig.ObstacleDatas)
                {
                    float targetCount = assetsConfig.AmountObstaclesInPool * ((float)obstacleData.ProportionInPool / 100);
                    for (var i = 0; i < targetCount; i++)
                    {
                        var obstacle = await LoadPrefab<ObstacleComponent>(obstacleData.ObstaclePrefab, cancellationToken);
                        obstacles.Add(obstacle);
                    }
                }
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
            SpawnedObstaclesCount++;
            return item;
        }

        public void Return(ObstacleComponent item)
        {
            if (item == null) return;

            // I prefer pools not to be responsible for disabling objects
            item.Deactivate();
            _obstaclesPool.ReturnToPool(item);
            SpawnedObstaclesCount--;
        }

        public override void Clear()
        {
            // Addressables handle dispose is in base 
            base.Clear();
 
            _obstaclesPool?.Dispose();
            _obstaclesPool = null;
        }

        private void ValidateMatchConfigAsserts(AssetsConfig config)
        {
            Debug.Assert(config != null, "AssetsConfig is null");
            Debug.Assert(config.ObstacleDatas.Length > 0, "Obstacles array is empty");

            int proportionSum = config.ObstacleDatas.Sum(obstacle => obstacle.ProportionInPool);
            Debug.Assert(proportionSum == 100, "Sum of obstacles proportions should be 100");
        }
    }
}