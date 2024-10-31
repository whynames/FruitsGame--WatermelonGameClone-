using UnityEngine;

namespace BotanGameServices.GameServices.Internal
{
    public class GameServicesExample : MonoBehaviour
    {
        public void Login()
        {
            BotanGameServices.GameServices.API.LogIn();
        }


        public void SubmitAchievement()
        {
            //you call should look like this, but we commented it because you will get errors if you define your own achievements
            //BotanGameServices.GameServices.API.SubmitAchievement(AchievementNames.LowJumper);

            //submit the first achievement from settings window
            BotanGameServices.GameServices.API.SubmitAchievement(0);
        }


        public void ShowAchievementsUI()
        {
            BotanGameServices.GameServices.API.ShowAchievementsUI();
        }


        long score = 100;
        public void SubmitScore()
        {
            //you call should look like this, but we commented it because you will get errors if you define your own LEADERBOARDS
            //BotanGameServices.GameServices.API.SubmitScore(score, LeaderboardNames.HighestJumpers);

            BotanGameServices.GameServices.API.SubmitScore(score, 0);
        }


        public void ShowLeaderboardsUI()
        {
            BotanGameServices.GameServices.API.ShowLeaderboadsUI();
        }
    }
}

