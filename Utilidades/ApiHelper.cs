using System.Text.Json;
using DTOs.Externos;

namespace Utilidades
{
    public static class ApiHelper
    {
        public static async Task<bool> ValidarCuentaExternaAsync(string numeroCuenta, string documento, string banco)
        {
            var url = $"https://apis.fluxis.com.co/apis/interbanks/accounts/?numeroCuenta={numeroCuenta}&documentoUsuario={documento}&banco={banco}";

            using var client = new HttpClient();

            try
            {
                var response = await client.GetAsync(url);
                var contenido = await response.Content.ReadAsStringAsync();

                var datos = JsonSerializer.Deserialize<RespuestaCuentaExterna>(contenido, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return datos?.status == "success";
            }
            catch
            {
                return false;
            }
        }




        public static async Task<decimal> ObtenerTRMAsync(DateTime fecha)
        {
            var fechaStr = fecha.ToString("yyyy-MM-dd");
            var url = $"https://trm-colombia.vercel.app/?date={fechaStr}";
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) throw new Exception("No se pudo obtener la TRM");

            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<JsonElement>(json);
            return obj.GetProperty("data").GetProperty("value").GetDecimal();
        }
    }

}
