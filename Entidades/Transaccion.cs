namespace Entidades
{
    public class Transaccion
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Numero { get; set; }
        public DateTime Fecha { get; set; } = DateTime.UtcNow;
        public decimal Monto { get; set; }
        public string Tipo { get; set; } // "entrada" o "salida"

        public Guid CuentaOrigenId { get; set; }
        public Cuenta CuentaOrigen { get; set; }

        public Guid CuentaDestinoId { get; set; }
        public Cuenta CuentaDestino { get; set; }
    }
}
