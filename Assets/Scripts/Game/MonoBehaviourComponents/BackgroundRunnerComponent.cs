using UnityEngine;
using UnityEngine.Serialization;

namespace Game.MonoBehaviourComponents
{
    public class BackgroundRunnerComponent : MonoBehaviour
    {
        [SerializeField] private Renderer _background;
        [SerializeField] private float _velocity = 2; 
        private bool _isRunning = false;

        private void Start()
        {
            Run();
        }

        public void Run()
        {
            Debug.Log("BackgroundRunnerComponent.Run");
        }
    
        public void Stop()
        {
            Debug.Log("BackgroundRunnerComponent.Stop");
        }

        private void Update()
        {
            _background.material.mainTextureOffset += new Vector2(0.015f, 0) * (_velocity * Time.deltaTime);
        }
    }
}
