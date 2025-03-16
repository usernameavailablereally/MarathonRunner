using System;
using UnityEngine;

namespace Game.Configs
{
    // ScriptableObject makes possible to easily edit parameters inside Unity 
    [CreateAssetMenu(fileName = "MatchConfig", menuName = "Configs/MatchConfig")]
    [Serializable]
    public class MatchConfig : ScriptableObject
    { 
        [Tooltip("General velocity of level movement")]
        public float MovingVelocity = 2;
        [Tooltip("How many obstacles is allowed on scene at once")]
        [Range(0, 10)]
        public int MaxObstaclesOnScene;
        [Tooltip("Minimum time interval for obstacles spawn (sec)")]
        [Range(0, 10)]
        public float SpawnObstacleInterval = 2f;
        [Tooltip("Possibility of obstacle spawn after Minimum interval")]
        [Range(0, 1)]
        public float SpawnObstacleThreshold = 0.5f;
    }
}