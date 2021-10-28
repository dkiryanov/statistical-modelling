using System;
using System.Collections.Generic;
using System.Text;

namespace WorkshopModelling
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine(@"Эмуляция работы цеха...");

            FileService fileService = new FileService();
            Global.Settings = fileService.GetSettingsFromXml();

            Business business = new Business();
            WareHouse wareHouse = new WareHouse();
            Bureau bureau = new Bureau();
            WorkShop workShop = new WorkShop();

            business.WorkShop = workShop;
            wareHouse.WorkShop = workShop;
            bureau.WareHouse = wareHouse;
            workShop.Bureau = bureau;

            IEnumerable<ITickable> callStack = new List<ITickable>()
            {
                business,
                workShop,
                bureau,
                wareHouse
            };

            Console.Write("\n");

            int marker = 0;

            for (int i = 0; i < Global.Settings.Time; i++)
            {
                Global.Step = i + 1;

                foreach (ITickable tickable in callStack)
                {
                    tickable.NextTick();
                }

                marker++;

                if (marker == 1000)
                {
                    Console.Write(".");
                    marker = 0;
                }
            }

            Console.WriteLine("\nРезультаты моделирования работы цеха:\n");

            Console.WriteLine($"Заявок выписано: {bureau.OrdersCount}");
            Console.WriteLine($"Деталей доставлено: {wareHouse.DetailsDeliveredCount}");
            Console.WriteLine($"Незавершенных заявок на комплектование: {wareHouse.ComplectDetailsActivities.Count}");
            Console.WriteLine($"Незавершенных доставок на склад: {wareHouse.DeliverDetailsActivities.Count}");
            Console.WriteLine($"Деталей выдано со склада: {business.DetailsGotCount}");
            Console.WriteLine($"Деталей осталось на складе: {workShop.Details.Count}");
            Console.WriteLine($"Средняя загрузка склада: {workShop.GetAverageDetailsCount()}");
            Console.WriteLine($"Время простоя (в минутах): {workShop.DownTime}");

            double downTimeProbability = (double)workShop.DownTime / Global.Settings.Time;
            Console.WriteLine($"Вероятность простоя: {downTimeProbability} ({downTimeProbability * 100}%)");

            Console.WriteLine("Нажмите 'Пробел' для выхода...");
            Console.ReadKey();
        }
    }
}
