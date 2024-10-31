#if BOTANGAMESERVICES_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://BotanGameServices.gitbook.io/rate-game/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Show Rate Popup bypassing the settings form Settings Window")]
    public class ForceShowRatePopup : FsmStateAction
    {
        public override void OnEnter()
        {
            BotanGameServices.RateGame.API.ForceShowRatePopup();
            Finish();
        }
    }
}
#endif
