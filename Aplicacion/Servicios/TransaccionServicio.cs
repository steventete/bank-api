using DTOs;
using DTOs.Externos;
using Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistencia.Contextos;
using System.Text.Json;
using Utilidades;

namespace Aplicacion.Servicios
{
    public class TransaccionServicio
    {
        private readonly BancoDbContext _context;
        private readonly IConfiguration _config;

        public TransaccionServicio(BancoDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<List<TransaccionDto>> ConsultarTransaccionesAsync(
            string cuentaOrigen,
            string? cuentaDestino,
            DateTime? fechaDesde,
            DateTime? fechaHasta)
        {
            var query = _context.Transacciones
                .Include(t => t.CuentaOrigen)
                .Include(t => t.CuentaDestino)
                .Where(t => t.CuentaOrigen.NumeroCuenta == cuentaOrigen)
                .AsQueryable();

            if (!string.IsNullOrEmpty(cuentaDestino))
                query = query.Where(t => t.CuentaDestino.NumeroCuenta == cuentaDestino);

            if (fechaDesde.HasValue)
                query = query.Where(t => t.Fecha >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(t => t.Fecha <= fechaHasta.Value);

            var transacciones = await query.OrderByDescending(t => t.Fecha).ToListAsync();

            return transacciones.Select(t => new TransaccionDto
            {
                Numero = t.Numero,
                Fecha = t.Fecha,
                CuentaOrigen = t.CuentaOrigen.NumeroCuenta,
                CuentaDestino = t.CuentaDestino.NumeroCuenta,
                Monto = t.Monto,
                Tipo = t.Tipo
            }).ToList();
        }

        public async Task<(bool Exito, string Mensaje)> RealizarTransaccionInternaAsync(string cuentaOrigen, CrearTransaccionDto dto)
        {
            var cuentaOrigenDb = await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == cuentaOrigen);

            if (cuentaOrigenDb == null)
                return (false, "Cuenta de origen no encontrada.");

            var cuentaDestinoDb = await _context.Cuentas
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c =>
                    c.NumeroCuenta == dto.Destino || c.Usuario.Numero == dto.Destino);

            if (cuentaDestinoDb == null)
                return (false, "Cuenta destino no encontrada (ni por número ni por teléfono).");

            if (cuentaOrigen == cuentaDestinoDb.NumeroCuenta)
                return (false, "No puedes transferir a tu misma cuenta.");

            if (dto.Monto < decimal.Parse(_config["Transacciones:MontoMinimo"]) || dto.Monto > decimal.Parse(_config["Transacciones:MontoMaximo"]))
                return (false, "El monto está fuera de los límites permitidos.");

            if (cuentaOrigenDb.Saldo < dto.Monto)
                return (false, "Saldo insuficiente.");

            // Ejecutar transacción
            cuentaOrigenDb.Saldo -= dto.Monto;
            cuentaDestinoDb.Saldo += dto.Monto;

            var numeroTransaccion = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);

            var transaccion = new Transaccion
            {
                Numero = numeroTransaccion,
                Fecha = DateTime.UtcNow,
                CuentaOrigenId = cuentaOrigenDb.Id,
                CuentaDestinoId = cuentaDestinoDb.Id,
                Monto = dto.Monto,
                Tipo = "salida"
            };

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return (true, "Transacción realizada correctamente.");
        }



        public static async Task<RespuestaCuentaExterna?> ValidarCuentaExternaAsync(string numeroCuenta, string documento, string banco)
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

                // Validar si el campo "status" es "success"
                if (datos?.status?.ToLower() != "success")
                    return null;

                return datos;
            }
            catch
            {
                return null;
            }
        }


        public async Task<(bool Exito, string Mensaje)> RealizarTransaccionInterbancariaAsync(string numeroCuentaOrigen, CrearTransaccionInterbancariaDto dto)
        {
            var cuentaOrigenDb = await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuentaOrigen);
            if (cuentaOrigenDb == null)
                return (false, "Cuenta de origen no encontrada.");

            if (cuentaOrigenDb.Saldo < dto.Monto)
                return (false, "Saldo insuficiente.");

            var datosExterno = await ValidarCuentaExternaAsync(dto.NumeroCuentaDestino, dto.NumeroDocumento, dto.BancoDestino);
            if (datosExterno == null)
                return (false, "Cuenta externa no válida.");

            decimal montoConvertido = dto.Monto;

            if (dto.Moneda.ToUpper() == "USD")
            {
                var trm = await ApiHelper.ObtenerTRMAsync(DateTime.UtcNow);
                if (trm <= 0)
                    return (false, "Error obteniendo la TRM.");

                montoConvertido = dto.Monto * trm;
            }

            cuentaOrigenDb.Saldo -= montoConvertido;

            string estado = dto.BancoDestino == "1007" ? "procesado" : "pendiente";

            var transaccion = new TransaccionInterbancaria
            {
                NumeroTransaccion = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12),
                TipoDocumento = dto.TipoDocumento,
                NumeroDocumento = dto.NumeroDocumento,
                NumeroCuentaDestino = dto.NumeroCuentaDestino,
                BancoDestino = dto.BancoDestino,
                Monto = dto.Monto,
                Moneda = dto.Moneda.ToUpper(),
                MontoConvertidoCOP = montoConvertido,
                Estado = estado,
                Fecha = DateTime.UtcNow,
                CuentaOrigenId = cuentaOrigenDb.Id
            };

            _context.TransaccionesInterbancarias.Add(transaccion);
            await _context.SaveChangesAsync();

            return (true, "Transacción interbancaria registrada correctamente.");
        }


        public async Task<(bool Exito, string Mensaje)> RecibirTransaccionInterbancariaAsync(string cuentaODestino, RecibirTransaccionInterbancariaDto dto)
        {
            var cuentaDestinoDb = await _context.Cuentas
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c =>
                    c.NumeroCuenta == cuentaODestino ||
                    c.Usuario.Numero == cuentaODestino);

            if (cuentaDestinoDb == null)
                return (false, "Cuenta destino no encontrada.");

            var datosExterno = await ApiHelper.ValidarCuentaExternaAsync(dto.NumeroCuentaOrigen, dto.NumeroDocumento, dto.BancoOrigen);
            if (datosExterno == null)
                return (false, "Cuenta externa no válida.");

            decimal montoConvertido = dto.Monto;

            if (dto.Moneda.ToUpper() == "USD")
            {
                var trm = await ApiHelper.ObtenerTRMAsync(DateTime.UtcNow);
                if (trm <= 0)
                    return (false, "Error obteniendo la TRM.");

                montoConvertido = dto.Monto * trm;
            }

            cuentaDestinoDb.Saldo += montoConvertido;

            var transaccion = new TransaccionInterbancaria
            {
                NumeroTransaccion = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12),
                TipoDocumento = dto.TipoDocumento,
                NumeroDocumento = dto.NumeroDocumento,
                NumeroCuentaDestino = cuentaDestinoDb.NumeroCuenta,
                BancoDestino = "NECLI",
                Monto = dto.Monto,
                Moneda = dto.Moneda.ToUpper(),
                MontoConvertidoCOP = montoConvertido,
                Estado = "procesado",
                Fecha = DateTime.UtcNow,
                CuentaOrigenId = cuentaDestinoDb.Id
            };

            _context.TransaccionesInterbancarias.Add(transaccion);
            await _context.SaveChangesAsync();

            return (true, "Transacción interbancaria recibida correctamente.");
        }



    }
}
