using System;
using System.Diagnostics;
using Windows.ApplicationModel.Contacts;
using Windows.Devices.Gpio;

namespace SprinklerWebApi
{

    internal class Signal2
    {
        public const byte Bit0 = 0xA0;
        public const byte Bit1 = 0x82;
        public const byte Sync = 0x80;

        public const int PulseMicroseconds = 275;
        public const long TicksPerPulse = 275*TimeSpan.TicksPerMillisecond/1000;

        

    }

    internal interface IBitSender
    {
        void Send(bool bit);
    }

    internal class GpioBitSender : IBitSender, IDisposable
    {
        private readonly GpioPin _pin;

        public GpioBitSender(GpioPin pin)
        {
            _pin = pin;
        }

        public void Send(bool bit)
        {
            _pin.Write(bit ? GpioPinValue.High : GpioPinValue.Low);
        }

        public void Dispose()
        {
            _pin.Dispose();
        }
    }


    internal class SignalReceiver
    {
        private readonly TimeSpan _silenceTimeout;

        public SignalReceiver(TimeSpan silenceTimeout)
        {
            _silenceTimeout = silenceTimeout;
        }


    }


    internal class Signal
    {

        public static void Send(int pinNumber, int data)
        {
            var controller = GpioController.GetDefault();
            using (var pin = controller.OpenPin(pinNumber, GpioSharingMode.Exclusive))
            {
                pin.Write(GpioPinValue.High);
                pin.SetDriveMode(GpioPinDriveMode.Output);

                Send(pin, data);
            }
        }

        private static void Send(GpioPin pin, int data)
        {
            for (var bitNumber = 0; bitNumber < 32; bitNumber++)
            {
                var currentBit = (data & (0x1 << bitNumber)) != 0;
                if (currentBit) Bit1(pin);
                else Bit0(pin);
            }
            Sync(pin);
        }

        //Bit 0: 275탎 high + 275탎 low + 275탎 high + 1225탎 low
        //  ______        ______
        //_|      |______|      |_____________
        public static void Bit0(GpioPin pin)
        {
            High(pin, 275);
            Low(pin, 275);
            High(pin, 275);
            Low(pin, 1225);
        }

        private static void High(GpioPin pin, int microseconds)
        {
            pin.Write(GpioPinValue.Low);
            Nop(microseconds);
        }

        private static void Low(GpioPin pin, int microseconds)
        {
            pin.Write(GpioPinValue.High);
            Nop(microseconds);
        }

        //Bit 1: 275탎 high + 1225탎 low + 275탎 high + 275탎 low
        //  ______               ______
        //_|      |_____________|      |______
        public static void Bit1(GpioPin pin)
        {

            High(pin, 275);
            Low(pin, 1225);
            High(pin, 275);
            Low(pin, 275);

        }

        //Bit sync: 275탎 high + 2675탎 low
        //  ______
        //_|      |________________________//_____________
        public static void Sync(GpioPin pin)
        {
            High(pin, 275);
            Low(pin, 2675);
        }

        private const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

        private static void Nop(int microseconds)
        {
            var ticks = microseconds * TicksPerMicrosecond;
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedTicks < ticks )
            {
                // unfortunatly thats the best way microseconds know to block such precisley.. :/
            }
        }
    }
}