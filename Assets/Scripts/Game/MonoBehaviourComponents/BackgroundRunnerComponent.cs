using UnityEngine;

namespace Game.MonoBehaviourComponents
{
    public class BackgroundRunnerComponent : MonoBehaviour
    {
        private const float MATCH_CONFIG_VELOCITY_DUMP_FACTOR = 2;
        [SerializeField] private Renderer _background;
        
        private bool _isActive;
        private readonly Vector2 _moveVector = new(0.015f, 0);
        private float _velocity;

        public void Activate(float matchVelocity)
        {
            if (this == null) return;

            _velocity = matchVelocity / MATCH_CONFIG_VELOCITY_DUMP_FACTOR;
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (this == null) return;
            
            _isActive = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isActive)
            {
                SetMaterialPositionTranslate(_moveVector * (_velocity * Time.deltaTime));
            }
        }

        private void SetMaterialPositionTranslate(Vector2 position)
        {
            _background.material.mainTextureOffset += position;
        }
    }
}