using System;
using NLog;

namespace WorkshopModelling
{
    public class Business : ITickable
    {
        private static Logger _logger;

        public WorkShop WorkShop { get; set; }

        public int DetailsGotCount { get; set; }

        public Activity GetDetailActivity { get; private set; }
        
        public Business()
        {
            _logger = LogManager.GetCurrentClassLogger();

            DetailsGotCount = 0;
            GetDetailActivity = GetNewActivity;
        }

        public void NextTick()
        {
            if (WorkShop == null)
            {
                throw new NullReferenceException();
            }

            if (IsNewDetailNeeded())
            {
                Detail detail = WorkShop.GetDetail();

                if (detail != null)
                {
                    DetailsGotCount += 1;

                    _logger.Info($"{Global.Step}: Затребован комплект" +
                                 $" деталей (успешно)");
                }
                else
                {
                    _logger.Info($"{Global.Step}: Затребован комплект " +
                                 $"деталей (неуспешно, простой)");
                }
            }
        }

        private bool IsNewDetailNeeded()
        {
            if (GetDetailActivity.IsActive())
            {
                return false;
            }

            GetDetailActivity = GetNewActivity;

            return true;
        }

        private Activity GetNewActivity => new Activity(
            Global.Settings.GetDetailActivity.MinTime, 
            Global.Settings.GetDetailActivity.MaxTime);
    }
}