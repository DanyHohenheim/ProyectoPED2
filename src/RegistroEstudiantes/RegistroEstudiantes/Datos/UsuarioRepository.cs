using Microsoft.Data.Sqlite;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes.Datos
{
    public static class UsuarioRepository
    {
        public static Usuario? ValidarUsuario(string usuario, string clave)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqliteCommand(
                "SELECT IdUsuario, NombreCompleto, NombreUsuario, Rol FROM Usuarios WHERE NombreUsuario=@u AND ClaveHash=@c AND Activo=1", conn))
            {
                cmd.Parameters.AddWithValue("@u", usuario);
                cmd.Parameters.AddWithValue("@c", clave);
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        return new Usuario
                        {
                            IdUsuario = rdr.GetInt32(0),
                            NombreCompleto = rdr.GetString(1),
                            NombreUsuario = rdr.GetString(2),
                            Rol = rdr.GetString(3)
                        };
                    }
                }
            }
            return null;
        }

        public static bool RegistrarUsuario(Usuario u)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqliteCommand(
                "INSERT INTO Usuarios (NombreCompleto, NombreUsuario, ClaveHash, Rol, Activo) VALUES (@nom, @usr, @clave, @rol, 1)", conn))
            {
                cmd.Parameters.AddWithValue("@nom", u.NombreCompleto);
                cmd.Parameters.AddWithValue("@usr", u.NombreUsuario);
                cmd.Parameters.AddWithValue("@clave", u.ClaveHash);
                cmd.Parameters.AddWithValue("@rol", u.Rol);
                try
                {
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch { return false; }
            }
        }

        public static bool ExisteUsuario(string nombreUsuario)
        {
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqliteCommand(
                "SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario=@u", conn))
            {
                cmd.Parameters.AddWithValue("@u", nombreUsuario);
                long count = (long)cmd.ExecuteScalar()!;
                return count > 0;
            }
        }
    }
}