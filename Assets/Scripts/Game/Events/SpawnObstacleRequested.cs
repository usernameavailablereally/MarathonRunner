using Core.Services.Events;
using Game.MonoBehaviourComponents.LoadingAssets;

namespace Game.Events
{
    public class SpawnObstacleRequested : GameEventBase
    {
        public readonly ObstacleComponent SpawnedObstacle;
        public SpawnObstacleRequested(ObstacleComponent spawnedObstacle)
        {
            SpawnedObstacle = spawnedObstacle;
        }
    }
}