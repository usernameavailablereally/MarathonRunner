using Core.Services.Events;
using Game.MonoBehaviourComponents;

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