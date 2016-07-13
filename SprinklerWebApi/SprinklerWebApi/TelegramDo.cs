using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net.Attributes;

namespace SprinklerWebApi
{
    public sealed class TelegramDo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTimeOffset ReceivedAt { get; set; }

        public byte[] TickBytes { get; set; }

        public int TicksPerSecond { get; set; }
    }
}