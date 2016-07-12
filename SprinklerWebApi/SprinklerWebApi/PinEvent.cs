using System;
using System.Threading;
using Windows.Devices.Gpio;

namespace SprinklerWebApi
{
    internal class PinEvent : IDisposable
    {
        private readonly GpioPin _pin;
        private readonly GpioPinEdge _edge;
        private readonly AutoResetEvent _event;

        public PinEvent(GpioPin pin, GpioPinEdge edge)
        {
            _pin = pin;
            _edge = edge;
            _event = new AutoResetEvent(false);
        }

        public bool WaitOne(TimeSpan timeout)
        {
            _pin.ValueChanged += Changed;
            return _event.WaitOne(timeout);
        }

        private void Changed(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge != _edge) return;
            _pin.ValueChanged -= Changed;
            _event.Set();
        }

        public void Dispose()
        {
            _event.Dispose();
        }
    }
}