using UnityEngine;

namespace Game.MonoBehaviourComponents
{
    public class BackgroundRunnerComponent : MonoBehaviour
    {
        [SerializeField] private Renderer _background;
        [SerializeField] private float _velocity = 2;
        private bool _isActive = false;
        public void Activate()
        {
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isActive = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_isActive)
            {
                _background.material.mainTextureOffset += new Vector2(0.015f, 0) * (_velocity * Time.deltaTime);
            }
        }
    }
}