using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    [Serializable]
    public class ObstacleData
    {
        public AssetReference ObstaclePrefab;
        [Tooltip("Proportion percentage of this obstacle in the pool (0-100)")]
        [Range(0, 100)]
        public int ProportionInPool;
    }
}