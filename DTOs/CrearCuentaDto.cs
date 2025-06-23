namespace DTOs
{
    public class CrearCuentaDto
    {
        public string Identificacion { get; set; }
        public string TipoUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Numero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Contrasena { get; set; }
    }
}
