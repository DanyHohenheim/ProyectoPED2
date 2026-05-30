// Modelos/Estudiante.cs
namespace RegistroEstudiantes.Modelos
{
    public class Estudiante
    {
        public int IdEstudiante { get; set; }
        public string Carnet { get; set; } = "";
        public string Nombres { get; set; } = "";
        public string Apellidos { get; set; } = "";
        public int IdCarrera { get; set; }
        public string Correo { get; set; } = "";
        public string Telefono { get; set; } = "";
        public bool Activo { get; set; } = true;
        public string CarreraNombre { get; set; } = ""; // Solo para mostrar
    }
}