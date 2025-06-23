using Entidades;
using Utilidades;
using Persistencia.Contextos;
using Microsoft.EntityFrameworkCore;

namespace Aplicacion.Servicios
{
    public class ResetPasswordServicio
    {
        private readonly BancoDbContext _context;
        private readonly CorreoService _correo;

        public ResetPasswordServicio(BancoDbContext context, CorreoService correo)
        {
            _context = context;
            _correo = correo;
        }

        public async Task<string> SolicitarResetAsync(string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return "No existe una cuenta con ese correo.";

            var token = Guid.NewGuid().ToString();
            var entidad = new ResetPasswordToken
            {
                Identificacion = usuario.Identificacion,
                Token = token,
                Expiracion = DateTime.UtcNow.AddMinutes(30)
            };

            _context.ResetPasswordTokens.Add(entidad);
            await _context.SaveChangesAsync();

            await _correo.EnviarCorreoAsync(email, "Restablecimiento de contraseña",
                $"Usa este token para restablecer tu contraseña: {token}");

            return "Correo enviado con instrucciones para restablecer contraseña.";
        }

        public async Task<string> RestablecerClaveAsync(string token, string nuevaClave)
        {
            var entrada = await _context.ResetPasswordTokens.FirstOrDefaultAsync(t =>
                t.Token == token && t.Expiracion > DateTime.UtcNow);

            if (entrada == null) return "Token inválido o expirado.";

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.Identificacion == entrada.Identificacion);

            if (usuario == null) return "Usuario no encontrado.";

            usuario.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(nuevaClave);

            // Limpiar token usado
            _context.ResetPasswordTokens.Remove(entrada);
            await _context.SaveChangesAsync();

            await _correo.EnviarCorreoAsync(usuario.Email, "Contraseña actualizada",
                "Tu contraseña fue actualizada correctamente.");

            return "Contraseña restablecida con éxito.";
        }
    }

}
