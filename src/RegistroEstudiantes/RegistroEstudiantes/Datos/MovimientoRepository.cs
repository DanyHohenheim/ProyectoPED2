using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes.Datos
{
    public static class MovimientoRepository
    {
        public static List<Movimiento> ObtenerTodos()
        {
            var lista = new List<Movimiento>();
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqliteCommand(
                @"SELECT M.IdMovimiento, E.Carnet, E.Nombres || ' ' || E.Apellidos, 
                  U.NombreCompleto, M.TipoMovimiento, M.FechaMovimiento, M.Descripcion
                  FROM MovimientosEstudiante M
                  INNER JOIN Estudiantes E ON M.IdEstudiante = E.IdEstudiante
                  INNER JOIN Usuarios U ON M.IdUsuario = U.IdUsuario
                  ORDER BY M.FechaMovimiento DESC", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    lista.Add(new Movimiento
                    {
                        IdMovimiento = rdr.GetInt32(0),
                        CarnetEstudiante = rdr.GetString(1),
                        NombreEstudiante = rdr.GetString(2),
                        NombreUsuario = rdr.GetString(3),
                        TipoMovimiento = rdr.GetString(4),
                        FechaMovimiento = rdr.GetString(5),
                        Descripcion = rdr.GetString(6)
                    });
                }
            }
            return lista;
        }
    }
}