using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Assets.Scripts.Misc
{
    public class Boot : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = CalculateMinimumRefreshRate((float)Screen.currentResolution.refreshRateRatio.value);
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }

        private static int CalculateMinimumRefreshRate(float refreshRate, float minRefreshRate = 40f)
        {
            if (refreshRate <= minRefreshRate)
                return (int)refreshRate;

            var divider = 0f;
            var lastValidDivider = 1f;
            const int limit = 1000;
            while (divider < limit)
            {
                divider += 1f;

                if (refreshRate % divider != 0f)
                    continue;

                var value = refreshRate / divider;
                if (value >= minRefreshRate)
                {
                    lastValidDivider = divider;
                    continue;
                }

                var result = Mathf.RoundToInt(refreshRate / lastValidDivider);
                return result;
            }

            return (int)refreshRate;
        }
    }
}