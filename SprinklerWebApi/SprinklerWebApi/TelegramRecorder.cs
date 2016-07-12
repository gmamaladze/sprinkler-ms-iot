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
                //pin.SetDriveMode(GpioPinDriveMode.Input);
                pin.WaitOne(GpioPinEdge.RisingEdge, timeout);
                var signal = new Queue<long>();
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var previous = GpioPinValue.High;
                while (true)
                {
                    var current = pin.Read();
                    if (stopwatch.Elapsed> silence) break;
                    if (token.IsCancellationRequested) break;
                    if (previous==current) continue;
                    previous = current;
                    signal.Enqueue(stopwatch.ElapsedTicks);
                    stopwatch.Restart();
                }

                return new Telegram
                {
                    ReceivedAt = DateTimeOffset.UtcNow,
                    TicksPerSecond = (int)TimeSpan.TicksPerSecond,
                    SignalTicks = string.Join(",", signal.Select(ticks=>ticks.ToString(CultureInfo.InvariantCulture)))
                };
            };
        }
    }
}