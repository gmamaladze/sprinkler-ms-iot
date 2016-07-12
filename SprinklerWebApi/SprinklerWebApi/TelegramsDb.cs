using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace SprinklerWebApi
{
    
    public sealed class TelegramsDb
    {
        private static SQLiteConnection GetConnection()
        {
            var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "telegrams.db");
            var conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
            conn.CreateTable<Telegram>();
            return conn;
        }

        public static Telegram Get(int id)
        {
            using (var conn = GetConnection())
                return conn
                    .Table<Telegram>()
                    .FirstOrDefault(t => t.Id == id);
        }

        public static IEnumerable<Telegram> GetAll()
        {
            using (var conn = GetConnection())
                return conn
                    .Table<Telegram>();
        }

        public static int Delete(int id)
        {
            using (var conn = GetConnection())
                return conn.Delete<int>(id);
        }

        public static int Clear()
        {
            using (var conn = GetConnection())
                return conn.DeleteAll<Telegram>();
        }

        public static int Add(Telegram telegram)
        {
            using (var conn = GetConnection())
                return conn.Insert(telegram);
        }
    }
}
