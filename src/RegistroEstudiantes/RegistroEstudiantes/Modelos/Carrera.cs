namespace RegistroEstudiantes.Modelos
{
    public class Carrera
    {
        public int IdCarrera { get; set; }
        public string CodigoCarrera { get; set; } = "";
        public string NombreCarrera { get; set; } = "";
        public string Facultad { get; set; } = "";
        public bool Activa { get; set; } = true;
    }
}
