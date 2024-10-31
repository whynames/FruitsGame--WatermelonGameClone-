#if BOTANGAMESERVICES_PLAYMAKER_SUPPORT

using BotanGameServices.DailyRewards;

namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://BotanGameServices.gitbook.io/daily-rewards/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Get button timer")]

    public class GetRemainingTime : FsmStateAction
    {
        public FsmEnum buttonToCheck;

        public FsmString fsmTime;

        public override void OnEnter()
        {
            base.OnEnter();
            fsmTime.Value = BotanGameServices.DailyRewards.API.TimerButton.GetRemainingTime((ButtonIdsForTimers)buttonToCheck.Value).ToString();
            Finish();
        }
    }
}
#endif
