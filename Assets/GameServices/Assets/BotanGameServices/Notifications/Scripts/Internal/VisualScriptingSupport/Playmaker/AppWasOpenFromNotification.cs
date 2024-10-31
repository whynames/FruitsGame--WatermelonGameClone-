#if BOTANGAMESERVICES_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://BotanGameServices.gitbook.io/mobile-notifications/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Check if app was opened from notification")]
    public class AppWasOpenFromNotification : FsmStateAction
    {
        [Tooltip("Event triggered if a notification was tapped")]
        public FsmEvent yes;

        [Tooltip("Event triggered if app was opened from icon")]
        public FsmEvent no;

        public override void OnEnter()
        {
            if (string.IsNullOrEmpty(BotanGameServices.Notifications.API.AppWasOpenFromNotification()))
            {
                Fsm.Event(FsmEventTarget.Self, no);
            }
            else
            {
                Fsm.Event(FsmEventTarget.Self, yes);
            }
            Finish();
        }
    }
}
#endif
