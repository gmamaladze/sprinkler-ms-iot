using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using Windows.Devices.Gpio;

namespace SprinklerWebApi
{
    internal static class TelegramRecorder
    {
        public static Telegram Read(int pinNumber, TimeSpan timeout, TimeSpan silence, CancellationToken token)
        {
            var controller = GpioController.GetDefault();
            using (var pin = controller.OpenPin(pinNumber, GpioSharingMode.SharedReadOnly))
            {
                pin.WaitOne(GpioPinEdge.RisingEdge, timeout);
                var signal = new Queue<long>();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var previous = GpioPinValue.High;
                while (true)
                {
                    var current = pin.Read();
                    if (previous == current)
                    {
                        if (stopwatch.Elapsed > silence) break;
                        if (token.IsCancellationRequested) break;
                        continue;
                    }
                    var ticks = stopwatch.ElapsedTicks;
                    stopwatch.Restart();
                    previous = current;
                    signal.Enqueue(ticks);
                }

                return new Telegram
                {
                    ReceivedAt = DateTimeOffset.UtcNow,
                    TicksPerSecond = Convert.ToInt32(TimeSpan.TicksPerSecond),
                    Ticks = signal.Select(Convert.ToUInt32).ToArray()
                };
            };
        }
    }
}