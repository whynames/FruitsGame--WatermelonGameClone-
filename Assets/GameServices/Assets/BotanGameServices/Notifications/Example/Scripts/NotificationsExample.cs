using UnityEngine;
using UnityEngine.UI;

namespace BotanGameServices.Notifications.Internal
{
    public class NotificationsExample : MonoBehaviour
    {


        bool sendNotification;
        [SerializeField]
        int notificationTime;
        [SerializeField]

        bool sendRepeatNotification;
        [SerializeField]

        int repeatNotificationTime;
        [SerializeField]

        bool sendBigPictureNotification;
        int bigPictureNotificationTime;

        [SerializeField]
        string nameTitle;

        [SerializeField]
        string repeatNotification;
        void Start()
        {
            BotanGameServices.Notifications.API.Initialize();
            //BotanGameServices.Notifications.API.CopyBigPictureToDevice("image.jpg");
        }



        /// <summary>
        /// The best way to schedule notifications is from OnApplicationFocus method
        /// when this is called user left your app
        /// when you trigger notifications when user is still in app, maybe your notification will be delivered when user is still inside the app and that is not good practice  
        /// </summary>
        /// <param name="focus"></param>
        private void OnApplicationFocus(bool focus)
        {
            if (focus == false)
            {
                if (sendNotification)
                {
                    BotanGameServices.Notifications.API.SendNotification(nameTitle, @"Why did the grape stop in the middle of the road?
Because it ran out of juice!", new System.TimeSpan(notificationTime, 0, 0), "icon_0", "icon_1", "Opened from BotanGameServices Notification");
                }
                if (sendRepeatNotification)
                {
                    BotanGameServices.Notifications.API.SendRepeatNotification(nameTitle, "Hey there! We miss your fruity expertise in our game! Come back and join the fun! There are new challenges waiting for you!", new System.TimeSpan(repeatNotificationTime, 0, 0), new System.TimeSpan(24, 0, 0), "icon_0", "icon_1", "Opened from BotanGameServices Repeat Notification");
                }
                if (sendBigPictureNotification)
                {
                    // BotanGameServices.Notifications.API.SendBigPictureNotification(nameTitle, "Big Picture Notification", "ComeBack", new System.TimeSpan(bigPictureNotificationTime, 0, 0), System.IO.Path.Combine(Application.persistentDataPath, "image.jpg"), false, "icon_0", "icon_1", "Opened from BotanGameServices Big Picture Notification");
                }
            }
            else
            {
                //call initialize when user returns to your app to cancel all pending notifications
                BotanGameServices.Notifications.API.CancelAllNotifications();
            }
        }
    }
}

