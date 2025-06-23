public class CrearTransaccionInterbancariaDto
{
    public string TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; }
    public string NumeroCuentaDestino { get; set; }
    public string BancoDestino { get; set; }
    public decimal Monto { get; set; }
    public string Moneda { get; set; } // COP o USD
}
