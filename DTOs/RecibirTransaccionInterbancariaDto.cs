namespace DTOs
{
    public class RecibirTransaccionInterbancariaDto
    {
        public string NumeroCuentaOrigen { get; set; } = string.Empty;
        public string NumeroCuentaDestino { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string BancoOrigen { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Moneda { get; set; } = "COP"; // Puede ser USD
    }
}
