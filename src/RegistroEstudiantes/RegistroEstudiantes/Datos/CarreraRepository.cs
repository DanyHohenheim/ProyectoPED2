using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using RegistroEstudiantes.Modelos;

namespace RegistroEstudiantes.Datos
{
    public static class CarreraRepository
    {
        public static List<Carrera> ObtenerTodas()
        {
            var lista = new List<Carrera>();
            using (var conn = DbHelper.GetConnection())
            using (var cmd = new SqliteCommand("SELECT IdCarrera, CodigoCarrera, NombreCarrera, Facultad, Activa FROM Carreras WHERE Activa=1", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    lista.Add(new Carrera
                    {
                        IdCarrera = rdr.GetInt32(0),
                        CodigoCarrera = rdr.GetString(1),
                        NombreCarrera = rdr.GetString(2),
                        Facultad = rdr.GetString(3),
                        Activa = rdr.GetInt32(4) == 1
                    });
                }
            }
            return lista;
        }
    }
}