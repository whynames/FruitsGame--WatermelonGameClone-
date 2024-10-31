using PrimeTween;
using TMPro;
using UnityEngine;
using VContainer;

namespace _Assets.Scripts.Services.UIs.InGame
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField]
        private ShakeSettings settings;
        [Inject] private ScoreService _scoreService;


        public void Init() => _scoreService.OnScoreChanged += ScoreChanged;
        private void ScoreChanged(int score)
        {
            Tween.PunchScale(scoreText.transform, settings).OnComplete(target: scoreText.transform, x => { x.localScale = Vector3.one; }, warnIfTargetDestroyed: false);
            scoreText.text = score.ToString();
        }


        private void OnDestroy() => _scoreService.OnScoreChanged -= ScoreChanged;
    }

}