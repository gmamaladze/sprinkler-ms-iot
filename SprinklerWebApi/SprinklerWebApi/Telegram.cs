using System;
using System.Collections;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace SprinklerWebApi
{
    public sealed class Telegram
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }

        public String SignalTicks { get; set; }

        public int TicksPerSecond { get; set; }
    }
}