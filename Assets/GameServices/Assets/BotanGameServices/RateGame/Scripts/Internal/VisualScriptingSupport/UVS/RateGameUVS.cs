#if BOTANGAMESERVICES_UVS_SUPPORT
using Unity.VisualScripting;

namespace BotanGameServices.RateGame.Internal
{
    [IncludeInSettings(true)]
    public static class RateGameUVS
    {
        public static void ShowRatePopup()
        {
            BotanGameServices.RateGame.API.ShowRatePopup();
        }

        public static void ForceShowRatePopup()
        {
            BotanGameServices.RateGame.API.ForceShowRatePopup();
        }

        public static void IncreaseCustomEvents()
        {
            BotanGameServices.RateGame.API.IncreaseCustomEvents();
        }
    }
}
#endif