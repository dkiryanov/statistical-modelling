using System;
using System.Collections.Generic;
using NLog;

namespace WorkshopModelling
{
    public class Bureau : ITickable
    {
        private static Logger _logger;

        public WareHouse WareHouse;

        public int OrdersCount { get; private set; }

        public Bureau()
        {
            _logger = LogManager.GetCurrentClassLogger();

            OrderActivities = new Dictionary<Guid, Activity>();
            OrdersCount = 0;
        }

        public Dictionary<Guid, Activity> OrderActivities { get; private set; }

        public void CreateOrder()
        {
            Activity activity = new Activity(
                Global.Settings.OrderActivity.MinTime,
                Global.Settings.OrderActivity.MaxTime);

            OrderActivities.Add(activity.Id, activity);
            OrdersCount++;

            _logger.Info($"{Global.Step}: Началось формирование заявки на " +
                         $"пополнение запасов цехового склада");
        }

        public void NextTick()
        {
            if (WareHouse == null)
            {
                throw new NullReferenceException();
            }

            List<Guid> idsToRemove = new List<Guid>();

            foreach (Activity activity in OrderActivities.Values)
            {
                if (activity.IsActive())
                {
                    continue;
                }

                idsToRemove.Add(activity.Id);
                WareHouse.CreateComplectDetailsActivity();

                _logger.Info($"{Global.Step}: Заявка на пополнение запасов цехового склада сформирована");
            }

            foreach (Guid id in idsToRemove)
            {
                OrderActivities.Remove(id);
            }
        }
    }
}