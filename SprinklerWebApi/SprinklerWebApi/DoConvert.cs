using System;
using System.Collections.Generic;
using System.Linq;

namespace SprinklerWebApi
{
    public static class DoConvert
    {
        public static TelegramDo ToDo(Telegram telegram)
        {
            return new TelegramDo
            {
                Id = telegram.Id,
                ReceivedAt = telegram.ReceivedAt,
                TicksPerSecond = telegram.TicksPerSecond,
                TickBytes = UInts2Bytes(telegram.Ticks)
            };
        }

        public static Telegram FromDo(TelegramDo telegramDo)
        {
            return new Telegram
            {
                Id = telegramDo.Id,
                ReceivedAt = telegramDo.ReceivedAt,
                Ticks = Bytes2Units(telegramDo.TickBytes).ToArray(),
                TicksPerSecond = telegramDo.TicksPerSecond
            };
        }

        private static IEnumerable<uint> Bytes2Units(byte[] bytes)
        {
            for (var i = 0; i < bytes.Length; i = i + 4)
            {
                yield return BitConverter.ToUInt32(bytes, i);
            }
        }

        private static byte[] UInts2Bytes(IEnumerable<uint> value)
        {
            return value
                .SelectMany(BitConverter.GetBytes)
                .ToArray();
        }
    }
}