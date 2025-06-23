namespace Entidades
{
    public class Cuenta
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string NumeroCuenta { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public bool Verificada { get; set; }

        public Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Transaccion> TransaccionesOrigen { get; set; }
        public ICollection<Transaccion> TransaccionesDestino { get; set; }
    }
}
