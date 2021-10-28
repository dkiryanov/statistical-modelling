using System;
using System.Collections.Generic;
using NLog;

namespace WorkshopModelling
{
    public class WorkShop : ITickable
    {
        private static Logger _logger;
        private double _detailSum;

        public WorkShop()
        {
            _logger = LogManager.GetCurrentClassLogger();

            Details = new Stack<Detail>();
            _detailSum = 0;

            for (int i = 0; i < Global.Settings.DetailsCount; i++)
            {
                Details.Push(new Detail());    
            }

            DownTime = 0;
        }

        public int DownTime { get; private set; }

        public Bureau Bureau { get; set; }

        public Stack<Detail> Details { get; set; }

        public Detail GetDetail()
        {
            if (Bureau == null)
            {
                throw new NullReferenceException();
            }

            if (Details.Count == 0)
            {
                return null;    
            }

            if (Details.Count == Global.Settings.Threshold)
            {
                Bureau.CreateOrder();
            }

            return Details.Pop();
        }

        public void NextTick()
        {
            _detailSum += Details.Count;

            if (Details.Count == 0)
            {
                DownTime += 1;

                _logger.Info($"{Global.Step}: Простой цеха");
            }
        }

        public int Refill()
        {
            int count = Details.Count;

            Details = new Stack<Detail>();

            for (int i = 0; i < Global.Settings.DetailsCount; i++)
            {
                Details.Push(new Detail());
            }

            return Global.Settings.DetailsCount - count;
        }

        public double GetAverageDetailsCount()
        {
            if (Global.Settings.Time == 0)
            {
                throw new ArgumentException("Argument cannot be null");
            }

            return _detailSum / Global.Settings.Time;
        }
    }
}