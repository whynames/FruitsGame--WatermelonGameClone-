#if BOTANGAMESERVICES_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://BotanGameServices.gitbook.io/rate-game/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Increase events for rate popup display")]
    public class IncreaseCustomEvents : FsmStateAction
    {
        public override void OnEnter()
        {
            BotanGameServices.RateGame.API.IncreaseCustomEvents();
            Finish();
        }
    }
}
#endif
