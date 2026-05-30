namespace RegistroEstudiantes.Modelos
{
    public class Movimiento
    {
        public int IdMovimiento { get; set; }
        public string NombreEstudiante { get; set; } = "";
        public string CarnetEstudiante { get; set; } = "";
        public string NombreUsuario { get; set; } = "";
        public string TipoMovimiento { get; set; } = "";
        public string FechaMovimiento { get; set; } = "";
        public string Descripcion { get; set; } = "";
    }
}