using RestoranLes1.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoranLes1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            Console.OutputEncoding = Encoding.UTF8;
            var rest = new Restaurant();
            while (true)
            {
                Console.WriteLine("Привет! Желаете забронировать столик?\n" +
                    "1 - мы уведомим Вас по смс (асинхронно)\n" +
                    "2 - подождите на линии, мы Вас оповестим(синхронно)\n" +
                    "Или снять бронь\n" +
                    "3 - мы уведомим Вас по смс (асинхронно)\n" +
                    "4 - подождите на линии, мы Вас оповестим(синхронно)\n"
                    );
                if (!int.TryParse(Console.ReadLine(), out var choice)
                    && choice != 1
                    && choice != 2
                    && choice != 3
                    && choice != 4)
                {
                    Console.WriteLine("Введите, пожалуйста 1, 2, 3 или 4");
                    continue;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                switch (choice)
                {
                    case 1:
                        rest.BookFreeTableAsync(1);
                        break;
                    case 2:
                        rest.BookFreeTable(1);
                        break;
                    case 3:
                        rest.BookBookedTable(rnd.Next(0, 9));
                        break;
                    case 4:
                        rest.BookBookedTableAsync(rnd.Next(0, 9));
                        break;
                }

                Console.WriteLine("Спасибо за Ваше обращение!");
                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
            }
        }
    }
}
