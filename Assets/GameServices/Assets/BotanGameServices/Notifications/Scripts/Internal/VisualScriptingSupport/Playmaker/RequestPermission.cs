#if BOTANGAMESERVICES_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://BotanGameServices.gitbook.io/mobile-notifications/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Request notification permission")]
    public class RequestPermission : FsmStateAction
    {
        [Tooltip("Event triggered if permission is granted")]
        public FsmEvent granted;

        [Tooltip("Event triggered if permission is denied")]
        public FsmEvent rejected;

        public override void OnEnter()
        {
            BotanGameServices.Notifications.API.RequestPermision(PermissionGranted);
        }

        private void PermissionGranted(bool hasPermission, string message)
        {
            if (!hasPermission)
            {
                Fsm.Event(FsmEventTarget.Self, rejected);
            }
            else
            {
                Fsm.Event(FsmEventTarget.Self, granted);
            }
            Finish();
        }
    }
}
#endif