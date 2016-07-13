using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace SprinklerWebApi
{
    public sealed class TelegramsDb : IDisposable
    {
        private readonly SQLiteConnection _conn;

        public TelegramsDb()
        {
            _conn = GetConnection();
        }

        public void Dispose()
        {
            _conn.Close();
            _conn.Dispose();
        }

        private static SQLiteConnection GetConnection()
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "telegrams.db");
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            conn.CreateTable<TelegramDo>();
            return conn;
        }

        public TelegramDo Get(int id)
        {
            return _conn
                .Table<TelegramDo>()
                .FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<TelegramDo> GetAll()
        {
            return _conn
                .Table<TelegramDo>();
        }

        public int Delete(int id)
        {
            return _conn.Delete<int>(id);
        }

        public int Clear()
        {
            return _conn.DeleteAll<TelegramDo>();
        }

        public int Add(TelegramDo telegramDo)
        {
            return _conn.Insert(telegramDo);
        }
    }
}