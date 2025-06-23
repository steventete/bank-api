namespace DTOs
{
    public class TransaccionDto
    {
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string CuentaOrigen { get; set; }
        public string CuentaDestino { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; }
    }
}
