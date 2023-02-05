using RestoranLes1.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RestoranLes1
{
    /// <summary>
    /// класс для взаимодействия с пользователем
    /// </summary>
    internal class SpeakerBot
    {
        private readonly Restaurant _restaraunt;
        private readonly TimeSpan interval = TimeSpan.FromMinutes(1);
        private const string _firstChoice = "1";
        private const string _secondChoice = "2";
        public SpeakerBot(Restaurant restaraunt)
        {
            _restaraunt = restaraunt;

            CancelBookingTimer(interval);
        }
        /// <summary>
        /// Метод для запуска взаимодействия с пользователем
        /// </summary>
        public void InitialHello()
        {
            while (true)
            {
                Console.WriteLine(Messages.InitialHelloChoice);

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                switch (Console.ReadLine())
                {
                    case _firstChoice:
                        ChooseTypeOfBooking();
                        break;
                    case _secondChoice:
                        ChooseTypeOfCancellation();
                        break;
                    default:
                        Console.WriteLine(Messages.Input1Or2);
                        break;
                }

                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
            }
        }

        private async void ChooseTypeOfBooking()
        {
            Console.WriteLine(Messages.BookingChoice);
            bool validInput = false;
            while (!validInput)
            {
                switch (Console.ReadLine())
                {
                    case _firstChoice:
                        validInput = true;
                        Console.WriteLine(Messages.WaitForBooking);
                        Console.WriteLine(Messages.YoullBeNotified);
                        var table = await _restaraunt.BookFreeTableAsync(1);
                        NotifyOnBooking(table);
                        break;
                    case _secondChoice:
                        validInput = true;
                        Console.WriteLine(Messages.WaitForBooking);
                        Console.WriteLine(Messages.StayOnLine);
                        table = _restaraunt.BookFreeTable(1);
                        Console.WriteLine(table is null
                            ? Messages.AllTablesOccupied
                            : Messages.BookingReady + table.Id);
                        break;
                    default:
                        Console.WriteLine(Messages.Input1Or2);
                        break;
                }
            }
        }

        private async void ChooseTypeOfCancellation()
        {
            Console.WriteLine(Messages.CancellationChoice);
            bool validInput = false;
            while (!validInput)
            {
                switch (Console.ReadLine())
                {
                    case _firstChoice:
                        validInput = true;
                        CancelAsync();
                        break;
                    case _secondChoice:
                        validInput = true;
                        Cancel();
                        break;
                    default:
                        Console.WriteLine(Messages.Input1Or2);
                        break;
                }
            }
        }

        private async void CancelAsync()
        {
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine(Messages.WaitForCancellation);
                if (!int.TryParse(Console.ReadLine(), out var tableId) || (tableId < 0 || tableId > 10))
                {
                    Console.WriteLine(Messages.InputTableId);
                }
                else if (!_restaraunt.CheckIfBooked(tableId))
                {
                    validInput = true;
                    Console.WriteLine(tableId + Messages.TableNotOccupied);
                }
                else
                {
                    validInput = true;
                    Console.WriteLine(Messages.YoullBeNotified);
                    var table = await _restaraunt.CancelBookingAsync(tableId);
                    NotifyOnCancellation(table);
                }
            }
        }

        private void Cancel()
        {
            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine(Messages.WaitForCancellation);
                if (!int.TryParse(Console.ReadLine(), out var tableId) || (tableId < 0 || tableId > 10))
                {
                    Console.WriteLine(Messages.InputTableId);
                }
                else if (!_restaraunt.CheckIfBooked(tableId))
                {
                    validInput = true;
                    Console.WriteLine(tableId + Messages.TableNotOccupied);
                }
                else
                {
                    validInput = true;
                    Console.WriteLine(Messages.StayOnLine);
                    var table = _restaraunt.CancelBooking(tableId);
                    Console.WriteLine(Messages.CancellationReady + table.Id);
                }
            }
        }

        private async void NotifyOnBooking(Table table)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine(table is null
                    ? Messages.Notification + Messages.AllTablesOccupied
                    : Messages.Notification + Messages.BookingReady + table.Id);
            });
        }
        private async void NotifyOnCancellation(Table table)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine(Messages.Notification + Messages.CancellationReady + table.Id);
            });
        }
        /// <summary>
        /// автоматически отменет бронь стола спустя какое-то время
        /// </summary>
        /// <param name="interval">интервал отмены брони</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CancelBookingTimer(TimeSpan interval, CancellationToken cancellationToken = default)
        {
            while (true)
            {
                var delayTask = Task.Delay(interval, cancellationToken);
                var cancelledTable = _restaraunt.CancelBookingTimed();
                if (cancelledTable != null)
                {
                    Console.WriteLine(Messages.AutoCancellation + cancelledTable.Id);
                }
                await delayTask;
            }
        }
    }
}
