using System;

namespace SprinklerWebApi
{
    public sealed class Telegram
    {
        public int Id { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }

        public uint[] Ticks { get; set; }

        public int TicksPerSecond { get; set; }
    }
}