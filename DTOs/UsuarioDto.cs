namespace DTOs
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Identificacion { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string TipoUsuario { get; set; } = null!;
    }
}
