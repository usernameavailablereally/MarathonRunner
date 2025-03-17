using Core.Services.Events;
using Game.MonoBehaviourComponents.LoadingAssets;

namespace Game.Events
{
    internal class ObstacleFinishedEvent : GameEventBase
    {
        public ObstacleComponent Obstacle;
        public ObstacleFinishedEvent(ObstacleComponent currentObstacle)
        {
            Obstacle = currentObstacle;
        }
    }
}