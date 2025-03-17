using Game.Configs;
using Game.MonoBehaviourComponents.LoadingAssets;
using UnityEngine;

namespace Game.MonoBehaviourComponents
{
    public class CameraControllerComponent : MonoBehaviour
    {
        private const float OBSTACLES_SPAWN_Y_DRAG_FACTOR = 1.1f;
        [SerializeField] private Transform _leftFinalizationTransform;
        [SerializeField] private Transform _columnDetectorTransform;
        [SerializeField] private Transform _obstacleSpawnerTransform;
        [SerializeField] private Transform _mainCameraTransform;

        private IGroundHitHandler _groundHitHandler;
        private IObstacleHitHandler _obstacleHitHandler;
        private MatchConfig _matchConfig;
        private bool _isActive;
        private int _targetLayerMask;

        public void Activate(IGroundHitHandler groundHitHandler, IObstacleHitHandler obstacleHitHandler, MatchConfig matchConfig)
        {
            _matchConfig = matchConfig;
            _groundHitHandler = groundHitHandler;
            _obstacleHitHandler = obstacleHitHandler;
            _isActive = true;
            _targetLayerMask = LayerMask.GetMask(StringConstants.GROUND_LAYER) | LayerMask.GetMask(StringConstants.OBSTACLE_LAYER); 
        }

        public void Deactivate()
        {
            _isActive = false;
            _matchConfig = null;
            _groundHitHandler.Dispose();
            _obstacleHitHandler.Dispose();
        }

        private void Update()
        {
            if (_isActive)
            {
                MoveCameraRight();
            }
        }

        private void FixedUpdate()
        {
            if (_isActive)
            {
                PerformRaycastFromFinalizingPoint();
            }
        }

        private void MoveCameraRight()
        {
            Vector3 newPosition = _mainCameraTransform.position + Vector3.right * (_matchConfig.MovingVelocity * Time.deltaTime);
            _mainCameraTransform.position = newPosition;
        }
 
        private void PerformRaycastFromFinalizingPoint()
        {
            Vector2 origin = _leftFinalizationTransform.position;
            Vector2 direction = Vector2.up; 

            PerformRaycastSeeker(origin, direction, out ColumnComponent column, out ObstacleComponent obstacle);
            if (column)
            {
                HandleColumnFinalization(column);
            }

            if (obstacle)
            {
                HandleObstacleFinalization(obstacle);
            }
        }

        private void HandleObstacleFinalization(ObstacleComponent finalizedObstacle)
        {
            _obstacleHitHandler.HandleObstacleHit(finalizedObstacle);
        }

        private void HandleColumnFinalization(ColumnComponent finalizedColumn)
        {
            ColumnComponent second = TryGetMaxRightColumn();
            _groundHitHandler.OnGroundHit(finalizedColumn, second);
        }

        private ColumnComponent TryGetMaxRightColumn()
        {
            Vector2 origin = _columnDetectorTransform.position;
            Vector2 direction = Vector2.left; 
            PerformRaycastSeeker(origin, direction, out ColumnComponent column, out ObstacleComponent _);
            return column;
        }

        private void PerformRaycastSeeker(Vector2 origin, Vector2 direction, out ColumnComponent column, out ObstacleComponent obstacle)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, _targetLayerMask);

            if (hit.collider)
            {
                Debug.DrawLine(origin, hit.point, Color.red); 
                
                column = hit.transform.GetComponent<ColumnComponent>(); 
                obstacle = hit.transform.GetComponent<ObstacleComponent>();
            }
            else
            {
                Debug.DrawLine(origin, origin + direction * 100, Color.green);
                column = null;
                obstacle = null;
            }
        }

        public void PlacePointsPositions(Vector3 leftPointPosition, Vector3 rightPointPosition)
        {
            _leftFinalizationTransform.position = leftPointPosition;
            _columnDetectorTransform.position = rightPointPosition;
            _obstacleSpawnerTransform.position = rightPointPosition + Vector3.up * OBSTACLES_SPAWN_Y_DRAG_FACTOR;
        }

        public Transform GetObstacleSpawnPoint()
        {
            return _obstacleSpawnerTransform;
        }

        public void ResetCameraPosition()
        {
            _mainCameraTransform.position = new Vector3(0, 0, -10);
        }
    }
}