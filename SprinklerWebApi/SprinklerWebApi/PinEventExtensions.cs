using System;
using Windows.Devices.Gpio;

namespace SprinklerWebApi
{
    public static class PinEventExtensions
    {
        public static bool WaitOne(this GpioPin pin, GpioPinEdge edge, TimeSpan timeout)
        {
            using (var pinEvent = new PinEvent(pin, edge))
            {
                return pinEvent.WaitOne(timeout);
            }
        }
    }
}