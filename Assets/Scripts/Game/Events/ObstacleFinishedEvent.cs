using Core.Services.Events;
using Game.MonoBehaviourComponents.LoadingAssets;

namespace Game.Events
{
    internal class ObstacleFinishedEvent : GameEventBase
    {
        public readonly ObstacleComponent Obstacle;
        public ObstacleFinishedEvent(ObstacleComponent currentObstacle)
        {
            Obstacle = currentObstacle;
        }
    }
}