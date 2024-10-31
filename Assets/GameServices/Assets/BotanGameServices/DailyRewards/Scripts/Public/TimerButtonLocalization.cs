#if BotanGameServices_LOCALIZATION
using BotanGameServices.Localization;
#endif
using UnityEngine;

namespace BotanGameServices.DailyRewards
{
        public class TimerButtonLocalization : MonoBehaviour
        {
#if BotanGameServices_LOCALIZATION
        public WordIDs ID;
#endif
                public string GetText()
                {
#if BotanGameServices_LOCALIZATION
            return Localization.API.GetText(ID);
#else
                        return null;
#endif
                }
        }
}