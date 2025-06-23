namespace Entidades
{
    public class Usuario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Identificacion { get; set; }
        public string TipoUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Numero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string ContrasenaHash { get; set; }

        public ICollection<Cuenta> Cuentas { get; set; }
    }
}
