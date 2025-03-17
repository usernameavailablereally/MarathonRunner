using Core.Services.Events;
using Game.MonoBehaviourComponents.LoadingAssets;

namespace Game.Events
{
    public class SpawnObstacleRequested : GameEventBase
    {
        public ObstacleComponent SpawnedObstacle;
        public SpawnObstacleRequested(ObstacleComponent spawnedObstacle)
        {
            SpawnedObstacle = spawnedObstacle;
        }
    }
}