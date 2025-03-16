using UnityEngine;

namespace Game.MonoBehaviourComponents
{
    public class ObstacleComponent : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        public void Activate()
        {
            _transform.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _transform.gameObject.SetActive(false);
        }

        public void TranslatePosition(Vector3 translationVector)
        {
            _transform.Translate(translationVector);
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            _transform.rotation = rotation;
        }
    }
}
