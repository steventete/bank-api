using Aplicacion.Servicios;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("transacciones")]
public class TransaccionesController : ControllerBase
{
    private readonly TransaccionServicio _servicio;

    public TransaccionesController(TransaccionServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] DateTime? fechaDesde, 
        [FromQuery] DateTime? fechaHasta, 
        [FromQuery] string? cuentaDestino)
    {
        var cuentaOrigen = User.Claims.FirstOrDefault(c => c.Type == "numeroCuenta")?.Value;
        if (string.IsNullOrEmpty(cuentaOrigen))
            return Unauthorized("Cuenta origen no identificada.");

        var resultado = await _servicio.ConsultarTransaccionesAsync(cuentaOrigen, cuentaDestino, fechaDesde, fechaHasta);
        return Ok(resultado);
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CrearTransaccionDto dto)
    {
        var cuentaOrigen = User.Claims.FirstOrDefault(c => c.Type == "numeroCuenta")?.Value;
        if (string.IsNullOrEmpty(cuentaOrigen))
            return Unauthorized("Cuenta origen no identificada.");

        var resultado = await _servicio.RealizarTransaccionInternaAsync(cuentaOrigen, dto);
        if (!resultado.Exito)
            return BadRequest(resultado.Mensaje);

        return Ok(resultado.Mensaje);
    }

    [HttpPost("interbancaria")]
    public async Task<IActionResult> PostInterbancaria([FromBody] CrearTransaccionInterbancariaDto dto)
    {
        var numeroCuenta = User.Claims.FirstOrDefault(c => c.Type == "numeroCuenta")?.Value;
        if (string.IsNullOrEmpty(numeroCuenta))
            return Unauthorized("Cuenta origen no identificada.");

        var (exito, mensaje) = await _servicio.RealizarTransaccionInterbancariaAsync(numeroCuenta, dto);
        if (!exito)
            return BadRequest(new { mensaje });

        return Ok(new { mensaje });
    }

    [AllowAnonymous]
    [HttpPost("interbancaria/recibir")]
    public async Task<IActionResult> RecibirInterbancaria([FromBody] RecibirTransaccionInterbancariaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.NumeroCuentaDestino))
            return BadRequest(new { mensaje = "Número de cuenta destino es requerido." });

        var (exito, mensaje) = await _servicio.RecibirTransaccionInterbancariaAsync(dto.NumeroCuentaDestino, dto);
        if (!exito)
            return BadRequest(new { mensaje });

        return Ok(new { mensaje });
    }

}
