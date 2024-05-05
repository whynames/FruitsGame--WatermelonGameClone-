using PrimeTween;
using TMPro;
using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Services.UIs.InGame
{
    public class InGameTimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TweenSettings<Vector3> scaleSettings;
        [SerializeField] private TweenSettings<Color> colorSettings;
        [Inject] private GameOverTimer _gameOverTimer;
        private float _effectTimer;

        public void Init()
        {
            _gameOverTimer.OnTimerStarted += TimerStarted;
            _gameOverTimer.OnTimeChanged += TimeChanged;
            _gameOverTimer.OnTimerStopped += TimerStopped;
            _gameOverTimer.OnTimerEnded += TimerEnded;
        }

        private void TimerStarted(float startTime, float currentTime) => timerText.text = currentTime.ToString("0.0");

        private void TimeChanged(float currentTime)
        {
            if (_effectTimer > 1)
            {
                _effectTimer = 0;
                Tween.Scale(this.transform, scaleSettings);
                Tween.Color(timerText, colorSettings);
            }
            _effectTimer += Time.deltaTime;
            timerText.text = currentTime.ToString("0.0");
        }

        private void TimerStopped(float startTime) => timerText.text = string.Empty;

        private void TimerEnded() => timerText.text = string.Empty;

        private void OnDestroy()
        {
            _gameOverTimer.OnTimerStarted -= TimerStarted;
            _gameOverTimer.OnTimeChanged -= TimeChanged;
            _gameOverTimer.OnTimerStopped -= TimerStopped;
            _gameOverTimer.OnTimerEnded -= TimerEnded;
        }
    }
}