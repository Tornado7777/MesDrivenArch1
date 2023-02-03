﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RestoranLes1.Model
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new List<Table>();
        public Restaurant() 
        {
            for (ushort i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столи и подтвержу вашу бронь, оставайтесь на линии!");
            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && 
                                    t.State == State.Free);
            Thread.Sleep(1000 * 5);
            table?.SetState(State.Booked);

            Console.WriteLine(table is null
                ? $"К сожалению все столики заняты"
                : $"Готово! Ваш столик номер {table.Id}.");
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столи и подтвержу вашу бронь, Вам придет уведомление!");
            Task.Run(async () => 
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons &&
                                   t.State == State.Free);
                await Task.Delay(1000 * 5);

                table?.SetState(State.Booked);

                Console.WriteLine(table is null
                    ? $"УВЕДОМЛЕНИ: К сожалению все столики заняты"
                    : $"УВЕДОМЛЕНИ: Готово! Ваш столик номер {table.Id}.");
            });           
        }

        public void BookBookedTable(int numberTable)
        {
            Console.WriteLine("Добрый день! Подождите секунду я сниму бронь, оставайтесь на линии!");
            var table = _tables.FirstOrDefault(t => t.Id == numberTable);
            Thread.Sleep(1000 * 5);

            table.SetState(State.Free);

            Console.WriteLine(table is null
                ? $"Столик с таким номером не найден."
                : $"Готово! Бронь снята со столика {table.Id}.");
        }

        public void BookBookedTableAsync(int numberTable)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столи и подтвержу вашу бронь, Вам придет уведомление!");
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.Id == numberTable);
                await Task.Delay(1000 * 5);

                table.SetState(State.Free);

                Console.WriteLine(table is null
                    ? $"УВЕДОМЛЕНИ: Столик с таким номером не найден."
                    : $"УВЕДОМЛЕНИ: Готово! Бронь снята со столика {table.Id}.");
            });
        }
    }
}
