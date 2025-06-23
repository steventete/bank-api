using Aplicacion.Servicios;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilidades;

namespace API.Controllers
{
    [ApiController]
    [Route("cuentas")]
    [Authorize]
    public class CuentasController : ControllerBase
    {
        private readonly CuentaServicio _cuentaServicio;
        private readonly CorreoService _correoService;

        public CuentasController(CuentaServicio cuentaServicio, CorreoService correoService)
        {
            _cuentaServicio = cuentaServicio;
            _correoService = correoService;
        }

        // 1. CREAR CUENTA
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CrearCuenta([FromBody] CrearCuentaDto dto)
        {
            var (mensaje, email, cuentaId) = await _cuentaServicio.CrearCuentaAsync(dto);

            if (mensaje.StartsWith("Cuenta creada"))
            {
                return Ok(new { mensaje });
            }
            else
            {
                return BadRequest(new { error = mensaje });
            }
        }

        // 2. CONSULTAR CUENTA
        [HttpGet("{numero}")]
        public async Task<IActionResult> ObtenerCuenta(string numero)
        {
            var cuenta = await _cuentaServicio.ObtenerCuentaPorNumeroAsync(numero);
            if (cuenta == null)
                return NotFound(new { error = "Cuenta no encontrada." });

            return Ok(cuenta);
        }

        // 3. ELIMINAR CUENTA
        [HttpDelete("{numero}")]
        public async Task<IActionResult> EliminarCuenta(string numero)
        {
            var resultado = await _cuentaServicio.EliminarCuentaAsync(numero);
            if (resultado == "Cuenta eliminada.")
                return Ok(new { mensaje = resultado });

            return BadRequest(new { error = resultado });
        }

        // 4. VERIFICAR CUENTA
        [HttpGet("verificar/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> VerificarCuenta(Guid id)
        {
            var resultado = await _cuentaServicio.VerificarCuentaAsync(id);
            if (resultado == "Cuenta verificada.")
                return Ok(new { mensaje = resultado });

            return BadRequest(new { error = resultado });
        }
    }
}
