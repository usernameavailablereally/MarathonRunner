using Core.Services.Events;
using Game.MonoBehaviourComponents.Objects;

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