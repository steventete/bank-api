namespace DTOs.Externos
{
    public class RespuestaCuentaExterna
    {
        public string status { get; set; }
        public string message { get; set; }
        public string? usuario { get; set; }
        public int? codigo_banco { get; set; }
        public string? banco_nombre { get; set; }
    }


}
