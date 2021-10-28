using System;

namespace WorkshopModelling
{
    public class Activity
    {
        public Guid Id { get; private set; }

        public int Time { get; private set; }

        public Activity(int minTime, int maxTime)
        {
            Random rnd = new Random();
            Time = rnd.Next(minTime, maxTime);
            Id = Guid.NewGuid();
        }

        public bool IsActive()
        {
            if (Time <= 0)
            {
                return false;
            }

            Time -= 1;

            return Time > 0;
        }
    }
}