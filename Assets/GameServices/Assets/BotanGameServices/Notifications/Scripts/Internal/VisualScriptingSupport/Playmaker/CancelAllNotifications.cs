#if BOTANGAMESERVICES_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://BotanGameServices.gitbook.io/mobile-notifications/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Cancel all pending notifications")]
    public class CancelAllNotifications : FsmStateAction
    {
        public override void OnEnter()
        {
            BotanGameServices.Notifications.API.CancelAllNotifications();
            Finish();
        }
    }
}
#endif
