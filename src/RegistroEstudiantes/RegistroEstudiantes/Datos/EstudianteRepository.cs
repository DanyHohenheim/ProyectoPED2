using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes.Datos
{
    public static class EstudianteRepository
    {
        public static List<Estudiante> ObtenerTodos()
        {
            var lista = new List<Estudiante>();
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqliteCommand(
                @"SELECT E.IdEstudiante, E.Carnet, E.Nombres, E.Apellidos, E.IdCarrera, C.NombreCarrera, E.Correo, E.Telefono, E.Activo 
                  FROM Estudiantes E INNER JOIN Carreras C ON E.IdCarrera = C.IdCarrera WHERE E.Activo=1", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    lista.Add(new Estudiante
                    {
                        IdEstudiante = rdr.GetInt32(0),
                        Carnet = rdr.GetString(1),
                        Nombres = rdr.GetString(2),
                        Apellidos = rdr.GetString(3),
                        IdCarrera = rdr.GetInt32(4),
                        CarreraNombre = rdr.GetString(5),
                        Correo = rdr.GetString(6),
                        Telefono = rdr.GetString(7),
                        Activo = rdr.GetInt32(8) == 1
                    });
                }
            }
            return lista;
        }

        public static List<Estudiante> ObtenerBajas()
        {
            var lista = new List<Estudiante>();
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqliteCommand(
                @"SELECT E.IdEstudiante, E.Carnet, E.Nombres, E.Apellidos, E.IdCarrera, C.NombreCarrera, E.Correo, E.Telefono, E.Activo 
                  FROM Estudiantes E INNER JOIN Carreras C ON E.IdCarrera = C.IdCarrera WHERE E.Activo=0", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    lista.Add(new Estudiante
                    {
                        IdEstudiante = rdr.GetInt32(0),
                        Carnet = rdr.GetString(1),
                        Nombres = rdr.GetString(2),
                        Apellidos = rdr.GetString(3),
                        IdCarrera = rdr.GetInt32(4),
                        CarreraNombre = rdr.GetString(5),
                        Correo = rdr.GetString(6),
                        Telefono = rdr.GetString(7),
                        Activo = rdr.GetInt32(8) == 1
                    });
                }
            }
            return lista;
        }

        public static bool Agregar(Estudiante e, int idUsuario)
        {
            using (var conn = DbHelper.GetConnection())
            {
                using (var cmd = new SqliteCommand(
                    "INSERT INTO Estudiantes (Carnet, Nombres, Apellidos, IdCarrera, Correo, Telefono, Activo) VALUES (@car, @nom, @ape, @idc, @mail, @tel, 1)", conn))
                {
                    cmd.Parameters.AddWithValue("@car", e.Carnet);
                    cmd.Parameters.AddWithValue("@nom", e.Nombres);
                    cmd.Parameters.AddWithValue("@ape", e.Apellidos);
                    cmd.Parameters.AddWithValue("@idc", e.IdCarrera);
                    cmd.Parameters.AddWithValue("@mail", e.Correo);
                    cmd.Parameters.AddWithValue("@tel", e.Telefono);
                    try
                    {
                        if (cmd.ExecuteNonQuery() <= 0) return false;
                    }
                    catch { return false; }
                }

                long nuevoId;
                using (var cmd2 = new SqliteCommand("SELECT last_insert_rowid()", conn))
                {
                    nuevoId = (long)cmd2.ExecuteScalar();
                }
                RegistrarMovimiento(conn, (int)nuevoId, idUsuario, "Alta", "Registro de nuevo estudiante");
            }
            return true;
        }

        public static bool Modificar(Estudiante e, int idUsuario)
        {
            using (var conn = DbHelper.GetConnection())
            {
                using (var cmd = new SqliteCommand(
                    "UPDATE Estudiantes SET Carnet=@car, Nombres=@nom, Apellidos=@ape, IdCarrera=@idc, Correo=@mail, Telefono=@tel WHERE IdEstudiante=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@car", e.Carnet);
                    cmd.Parameters.AddWithValue("@nom", e.Nombres);
                    cmd.Parameters.AddWithValue("@ape", e.Apellidos);
                    cmd.Parameters.AddWithValue("@idc", e.IdCarrera);
                    cmd.Parameters.AddWithValue("@mail", e.Correo);
                    cmd.Parameters.AddWithValue("@tel", e.Telefono);
                    cmd.Parameters.AddWithValue("@id", e.IdEstudiante);
                    try
                    {
                        if (cmd.ExecuteNonQuery() <= 0) return false;
                    }
                    catch { return false; }
                }
                RegistrarMovimiento(conn, e.IdEstudiante, idUsuario, "Actualizacion", "Modificación de datos del estudiante");
            }
            return true;
        }

        public static bool Baja(int idEstudiante, int idUsuario)
        {
            using (var conn = DbHelper.GetConnection())
            {
                using (var cmd = new SqliteCommand(
                    "UPDATE Estudiantes SET Activo=0 WHERE IdEstudiante=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", idEstudiante);
                    try
                    {
                        if (cmd.ExecuteNonQuery() <= 0) return false;
                    }
                    catch { return false; }
                }
                RegistrarMovimiento(conn, idEstudiante, idUsuario, "Baja", "Baja de estudiante");
            }
            return true;
        }

        private static void RegistrarMovimiento(SqliteConnection conn, int idEstudiante, int idUsuario, string tipo, string descripcion)
        {
            using (var cmd = new SqliteCommand(
                "INSERT INTO MovimientosEstudiante (IdEstudiante, IdUsuario, TipoMovimiento, Descripcion) VALUES (@ide, @idu, @tipo, @desc)", conn))
            {
                cmd.Parameters.AddWithValue("@ide", idEstudiante);
                cmd.Parameters.AddWithValue("@idu", idUsuario);
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@desc", descripcion);
                cmd.ExecuteNonQuery();
            }
        }
    }
}