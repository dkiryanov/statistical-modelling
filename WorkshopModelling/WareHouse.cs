using System;
using System.Collections.Generic;
using NLog;

namespace WorkshopModelling
{
    public class WareHouse : ITickable
    {
        private static Logger _logger;

        public WorkShop WorkShop { get; set; }

        public int DetailsDeliveredCount { get; set; }

        public Dictionary<Guid, Activity> ComplectDetailsActivities { get; private set; }

        public Dictionary<Guid, Activity> DeliverDetailsActivities { get; private set; }

        public WareHouse()
        {
            _logger = LogManager.GetCurrentClassLogger();

            DetailsDeliveredCount = 0;
            ComplectDetailsActivities = new Dictionary<Guid, Activity>();
            DeliverDetailsActivities = new Dictionary<Guid, Activity>();
        }

        public Activity CreateComplectDetailsActivity()
        {
            Activity activity = new Activity(
                Global.Settings.ComplectDetailsActivity.MinTime, 
                Global.Settings.ComplectDetailsActivity.MaxTime);

            ComplectDetailsActivities.Add(activity.Id, activity);

            _logger.Info($"{Global.Step}: Начато комплектование " +
                         $"заявки на пополнение запасов");

            return activity;
        }

        public void NextTick()
        {
            ProcessComplectDetailsActivities();
            ProcessDeliverDetailsActivities();
        }

        private void ProcessComplectDetailsActivities()
        {
            List<Guid> idsToRemove = new List<Guid>();

            foreach (Activity activity in ComplectDetailsActivities.Values)
            {
                if (activity.IsActive())
                {
                    continue;
                }
                
                idsToRemove.Add(activity.Id);

                Activity deliverActivity = new Activity(
                    Global.Settings.DeliverActivity.MinTime, 
                    Global.Settings.DeliverActivity.MaxTime);

                DeliverDetailsActivities.Add(deliverActivity.Id, deliverActivity);

                _logger.Info($"{Global.Step}: Завершено комплектование заявки на пополнение запасов");
                _logger.Info($"{Global.Step}: Началась доставка на склад");
            }

            foreach (Guid id in idsToRemove)
            {
                ComplectDetailsActivities.Remove(id);
            }
        }

        private void ProcessDeliverDetailsActivities()
        {
            if (WorkShop == null)
            {
                throw new NullReferenceException();
            }

            List<Guid> idsToRemove = new List<Guid>();

            foreach (Activity activity in DeliverDetailsActivities.Values)
            {
                if (activity.IsActive())
                {
                    continue;
                }

                idsToRemove.Add(activity.Id);

                int detailsCount = WorkShop.Refill();
                DetailsDeliveredCount += detailsCount;

                _logger.Info($"{Global.Step}: Завершена доставка в цех ({detailsCount} деталей)");
            }

            foreach (Guid id in idsToRemove)
            {
                DeliverDetailsActivities.Remove(id);
            }
        }
    }
}