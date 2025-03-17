using System;
using TMPro;
using UnityEngine;

namespace Game.MonoBehaviourComponents
{
    public class RoundTimerComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        private float _elapsedTime;
        private bool _isRunning;

        private void Update()
        {
            if (!_isRunning) return;
            
            _elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }

        public void Activate()
        {
            _elapsedTime = 0f;
            _isRunning = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            if (this == null) return;
            
            _isRunning = false;
            gameObject.SetActive(false);
        }

        private void UpdateTimerText()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_elapsedTime);
            _timerText.text = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds:D2}";
        }
    }
}