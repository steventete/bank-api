using DTOs;
using Entidades;
using Microsoft.EntityFrameworkCore;
using Persistencia.Contextos;

namespace Aplicacion.Servicios
{
    public class UsuarioServicio
    {
        private readonly BancoDbContext _context;

        public UsuarioServicio(BancoDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioDto>> ObtenerTodosAsync()
        {
            return await _context.Usuarios
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    Identificacion = u.Identificacion,
                    Nombres = u.Nombres,
                    Apellidos = u.Apellidos,
                    Email = u.Email,
                    Numero = u.Numero,
                    TipoUsuario = u.TipoUsuario
                })
                .ToListAsync();
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(Guid id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return null;

            return new UsuarioDto
            {
                Id = u.Id,
                Identificacion = u.Identificacion,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Email = u.Email,
                Numero = u.Numero,
                TipoUsuario = u.TipoUsuario
            };
        }

        public async Task<UsuarioDto?> ObtenerPorIdentificacionAsync(string identificacion)
        {
            var u = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Identificacion == identificacion);

            if (u == null) return null;

            return new UsuarioDto
            {
                Id = u.Id,
                Identificacion = u.Identificacion,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Email = u.Email,
                Numero = u.Numero,
                TipoUsuario = u.TipoUsuario
            };
        }


        public async Task<string> ActualizarAsync(Guid id, ActualizarUsuarioDto dto)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return "Usuario no encontrado.";

            u.Nombres = dto.Nombres;
            u.Apellidos = dto.Apellidos;
            u.Numero = dto.Numero;
            u.Email = dto.Email;

            await _context.SaveChangesAsync();
            return "Usuario actualizado.";
        }

        public async Task<string> EliminarAsync(Guid id)
        {
            var u = await _context.Usuarios.FindAsync(id);
            if (u == null) return "Usuario no encontrado.";

            _context.Usuarios.Remove(u);
            await _context.SaveChangesAsync();
            return "Usuario eliminado.";
        }
    }
}
