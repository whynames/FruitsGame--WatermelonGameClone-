using UnityEngine;
using UnityEngine.UI;

namespace BotanGameServices.AdsPackage.Local
{
    public class LogWindow : MonoBehaviour
    {
        [SerializeField] private Text logText;
        bool showWindow;

        private void OnEnable()
        {
            if (showWindow == false)
            {
                gameObject.SetActive(false);
            }
            BotanGameServicesLogger.onLogUpdate = ShowLogs;
        }

        public void HideLogsWindow()
        {
            showWindow = false;
            gameObject.SetActive(false);
        }

        public void ShowLogWindow()
        {
            showWindow = true;
            ShowLogs();
            gameObject.SetActive(true);
        }

        public void ClearLogs()
        {
            BotanGameServicesLogger.ClearLogs();
        }

        void ShowLogs()
        {
            logText.text = BotanGameServicesLogger.GetLogs();
        }
    }
}
