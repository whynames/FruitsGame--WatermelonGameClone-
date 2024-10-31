#if BOTANGAMESERVICES_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://BotanGameServices.gitbook.io/rate-game/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Show Rate Popup")]
    public class ShowRatePopup : FsmStateAction
    {
        public override void OnEnter()
        {
            BotanGameServices.RateGame.API.ShowRatePopup();
            Finish();
        }
    }
}
#endif
