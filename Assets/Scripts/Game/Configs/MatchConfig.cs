using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    // ScriptableObject makes possible to easily edit parameters inside Unity 
    [CreateAssetMenu(fileName = "MatchConfig", menuName = "Configs/MatchConfig")]
    [Serializable]
    public class MatchConfig : ScriptableObject
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
        [Tooltip("How many obstacles is allowed on scene at once")]
        [Range(0, 10)]
        public int MaxObstaclesOnScene;
    }
}