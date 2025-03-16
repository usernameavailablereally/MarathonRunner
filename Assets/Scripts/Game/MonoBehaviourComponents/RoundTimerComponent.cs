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

        public void StartTimer()
        {
            _elapsedTime = 0f;
            _isRunning = true;
        }

        public void StopTimer()
        {
            _isRunning = false;
        }

        private void UpdateTimerText()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_elapsedTime);
            _timerText.text = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}