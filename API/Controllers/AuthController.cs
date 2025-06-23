using Aplicacion.Servicios;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthServicio _authServicio;

        public AuthController(AuthServicio authServicio)
        {
            _authServicio = authServicio;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authServicio.LoginAsync(dto);
            if (token == null)
                return Unauthorized(new { error = "Credenciales inválidas." });

            return Ok(new { token });
        }

        [HttpPost("solicitar-restablecimiento")]
        public async Task<IActionResult> SolicitarRestablecimiento([FromBody] SolicitudResetDto dto)
        {
            var resultado = await _authServicio.EnviarTokenResetPasswordAsync(dto.Identificacion);
            if (!resultado.Exito)
                return BadRequest(new { error = resultado.Mensaje });

            return Ok(new { mensaje = resultado.Mensaje });
        }



        [HttpPost("confirmar-restablecimiento")]
        public async Task<IActionResult> ConfirmarRestablecimiento([FromBody] ConfirmarResetDto dto)
        {
            var resultado = await _authServicio.ConfirmarResetPasswordAsync(dto.Token, dto.NuevaClave);
            if (!resultado.Exito) return BadRequest(new { error = resultado.Mensaje });
            return Ok(new { mensaje = "Contraseña restablecida correctamente." });
        }

    }
}
