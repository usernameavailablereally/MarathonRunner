using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{   
    // ScriptableObject makes possible to easily edit parameters inside Unity 
    [CreateAssetMenu(fileName = "AssetsConfig", menuName = "Configs/AssetsConfig")]
    [Serializable]
    public class AssetsConfig : ScriptableObject
    {
        [Tooltip("Player prefab")]
        public AssetReference RunnerPrefab;
        [Tooltip("Columns prefabs")]   
        public AssetReference ColumnPrefab;
        [Tooltip("Obstacle data")]   
        public ObstacleData[] ObstacleDatas; 
        [Tooltip("Amount of obstacles in the pool. \n " +
                 "ObstaclesPrefabs will be duplicated to reach this amount")] 
        [Range(0, 100)]
        public int AmountObstaclesInPool;
    }
}