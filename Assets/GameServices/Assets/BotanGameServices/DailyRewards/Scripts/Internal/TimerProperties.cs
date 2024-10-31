using System;

namespace BotanGameServices.DailyRewards.Internal
{
    /// <summary>
    /// Helping class used to deal with multiple buttons in the same time
    /// </summary>
    public class TimerProperties
    {
        public ButtonIdsForTimers buttonID;
        public DateTime savedTime;
        public TimeSpan timeToPass;

        public TimerProperties(ButtonIdsForTimers buttonID)
        {
            this.buttonID = buttonID;
        }
    }
}
