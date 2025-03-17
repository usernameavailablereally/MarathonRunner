using UnityEngine;

namespace Game.MonoBehaviourComponents
{
    public class BackgroundRunnerComponent : MonoBehaviour
    {
        [SerializeField] private Renderer _background;
        
        private bool _isActive;
        private readonly Vector2 _moveVector = new(0.015f, 0);
        private float _velocity;

        public void Activate(float velocity)
        {
            if (this == null) return;

            _velocity = velocity;
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