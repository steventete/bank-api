using DTOs;
using Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistencia.Contextos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utilidades;

namespace Aplicacion.Servicios
{
    public class AuthServicio
    {
        private readonly BancoDbContext _context;
        private readonly IConfiguration _config;
        private readonly CorreoService _correoService;

        public AuthServicio(BancoDbContext context, IConfiguration config, CorreoService correoService)
        {
            _context = context;
            _config = config;
            _correoService = correoService;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
                return null;

            var hash = SeguridadUtilidades.ObtenerHash(dto.Contrasena);

            if (usuario.ContrasenaHash != hash)
                return null;

            // Buscar la cuenta asociada
            var cuenta = await _context.Cuentas.FirstOrDefaultAsync(c => c.UsuarioId == usuario.Id);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim("nombre", usuario.Nombres)
    };

            // Si tiene cuenta, agregar el número de cuenta al token
            if (cuenta != null)
            {
                claims.Add(new Claim("numeroCuenta", cuenta.NumeroCuenta));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<(bool Exito, string Mensaje)> EnviarTokenResetPasswordAsync(string identificacion)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Identificacion == identificacion);

            if (usuario == null)
                return (false, "Usuario no encontrado.");

            var token = Guid.NewGuid().ToString();

            _context.ResetPasswordTokens.Add(new ResetPasswordToken
            {
                Id = Guid.NewGuid(),
                Identificacion = identificacion,
                Token = token,
                Expiracion = DateTime.UtcNow.AddMinutes(30)
            });

            await _context.SaveChangesAsync();

            // Enviar correo usando CorreoService
            var asunto = "Restablecimiento de contraseña";
            var cuerpo = $"Tu token para restablecer la contraseña es: {token}. El token expira en 30 minutos.";

            try
            {
                await _correoService.EnviarCorreoAsync(usuario.Email, asunto, cuerpo);
            }
            catch (Exception ex)
            {
                return (false, $"Error enviando correo: {ex.Message}");
            }

            return (true, "Correo enviado.");
        }


        public async Task<(bool Exito, string Mensaje)> ConfirmarResetPasswordAsync(string token, string nuevaClave)
        {
            // Buscar el token activo que coincida y no haya expirado
            var resetToken = await _context.ResetPasswordTokens
                .FirstOrDefaultAsync(t => t.Token == token && t.Expiracion > DateTime.UtcNow);

            if (resetToken == null)
                return (false, "Token inválido o expirado.");

            // Buscar el usuario con la identificación guardada en el token
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Identificacion == resetToken.Identificacion);

            if (usuario == null)
                return (false, "Usuario no encontrado.");

            // Actualizar la contraseña (guardando el hash)
            usuario.ContrasenaHash = SeguridadUtilidades.ObtenerHash(nuevaClave);

            // Eliminar el token para que no pueda usarse de nuevo
            _context.ResetPasswordTokens.Remove(resetToken);

            await _context.SaveChangesAsync();

            return (true, "Contraseña restablecida correctamente.");
        }


    }
}
