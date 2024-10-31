#if BOTANGAMESERVICES_PLAYMAKER_SUPPORT
namespace HutongGames.PlayMaker.Actions
{
    [HelpUrl("https://BotanGameServices.gitbook.io/mobile-notifications/")]
    [ActionCategory(ActionCategory.ScriptControl)]
    [Tooltip("Initialize Notifications")]
    public class InitializeNotifications : FsmStateAction
    {
        public override void OnEnter()
        {
            BotanGameServices.Notifications.API.Initialize();
            Finish();
        }
    }
}
#endif
