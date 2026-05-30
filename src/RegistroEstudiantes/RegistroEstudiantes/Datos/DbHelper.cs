using Microsoft.Data.Sqlite;

namespace RegistroEstudiantes.Datos
{
    public static class DbHelper
    {
        public const string DbFile = "registro.db";

        public static SqliteConnection GetConnection()
        {
            // Microsoft.Data.Sqlite crea el archivo automáticamente, no necesita CreateFile
            var conn = new SqliteConnection($"Data Source={DbFile}");
            conn.Open();
            using (var cmd = new SqliteCommand("PRAGMA foreign_keys = ON;", conn))
            {
                cmd.ExecuteNonQuery();
            }
            return conn;
        }
    }
}