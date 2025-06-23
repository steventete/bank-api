using Aplicacion.Servicios;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("usuarios")]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioServicio _usuarioServicio;

        public UsuariosController(UsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarios = await _usuarioServicio.ObtenerTodosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(Guid id)
        {
            var usuario = await _usuarioServicio.ObtenerPorIdAsync(id);
            if (usuario == null) return NotFound(new { error = "Usuario no encontrado." });

            return Ok(usuario);
        }

        [HttpGet("identificacion/{identificacion}")]
        public async Task<IActionResult> ObtenerPorIdentificacion(string identificacion)
        {
            var usuario = await _usuarioServicio.ObtenerPorIdentificacionAsync(identificacion);
            if (usuario == null) return NotFound(new { error = "Usuario no encontrado." });

            return Ok(usuario);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarUsuarioDto dto)
        {
            var resultado = await _usuarioServicio.ActualizarAsync(id, dto);
            if (resultado == "Usuario actualizado.") return Ok(new { mensaje = resultado });

            return NotFound(new { error = resultado });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(Guid id)
        {
            var resultado = await _usuarioServicio.EliminarAsync(id);
            if (resultado == "Usuario eliminado.") return Ok(new { mensaje = resultado });

            return NotFound(new { error = resultado });
        }
    }
}
