using System;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;

namespace SprinklerWebApi
{
    internal class Command
    {
        public Command(uint address, byte unit, bool state, bool all)
        {
            if (address>=1<<26) throw new ArgumentOutOfRangeException();
            Data = (int)address;
            Data = Data | (all ? 1 : 0 << 26);
            Data = Data | (state ? 1 : 0 << 27);
            Data = Data | (unit & 0xF) << 28;
        }

        public int Data { get; }
    }
}