using UnityEngine;

namespace BotanGameServices.RateGame.Internal
{
    public class RateGameExample : MonoBehaviour
    {
        /// <summary>
        /// Show Rate Game Popup every time this script starts and conditions are met
        /// </summary>
        private void Start()
        {
            BotanGameServices.RateGame.RateGame.ShowRatePopupWithCallback(PopupClosedMethod);
        }

        /// <summary>
        /// Increase custom event by pressing UI Button
        /// </summary>
        public void IncreaseCustomEvents()
        {
            BotanGameServices.RateGame.RateGame.IncreaseCustomEvents();
        }


        /// <summary>
        /// Show Rate Game Popup even if conditions are not met by pressing the UI Button
        /// </summary>
        public void ForceShowPopup()
        {
            BotanGameServices.RateGame.RateGame.ForceShowRatePopupWithCallback(PopupClosedMethod);
            //BotanGameServices.RateGame.API.ShowNativeRatePopup();
        }


        /// <summary>
        /// Triggered when Rate Popup is closed
        /// </summary>
        private void PopupClosedMethod(BotanGameServices.RateGame.PopupOptions result, string message)
        {
            Debug.Log($"Popup Closed-> Result: {result}, Message: {message} -> Resume Game");
        }
    }
}