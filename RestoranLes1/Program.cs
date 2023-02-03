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
            Console.OutputEncoding= Encoding.UTF8;
            var rest = new Restaurant();
            while(true)
            {
                Console.WriteLine("Привет! Желаете забронировать столик?\n" +
                    "1 - мы уведомим Вас по смс (асинхронно)\n" +
                    "2 - подождите на линии, мы Вас оповестим(синхронно)");
                if(!int.TryParse(Console.ReadLine(), out var choice) && choice != 1 && choice != 2)
                {
                    Console.WriteLine("Введите, пожалуйста 1 или 2");
                    continue;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                if(choice== 1)
                {
                    rest.BookFreeTableAsync(1);
                }
                else
                {
                    rest.BookFreeTable(1);
                }

                Console.WriteLine("Спасибо за Ваше обращение!");
                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
            }
        }
    }
}
