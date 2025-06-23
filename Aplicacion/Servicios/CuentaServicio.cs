using DTOs;
using Entidades;
using Microsoft.EntityFrameworkCore;
using Persistencia.Contextos;
using System.Security.Cryptography;
using System.Text;
using Utilidades;

namespace Aplicacion.Servicios
{
    public class CuentaServicio
    {
        private readonly BancoDbContext _context;
        private readonly CorreoService _correoService;

        public CuentaServicio(BancoDbContext context, CorreoService correoService)
        {
            _context = context;
            _correoService = correoService;
        }

        // 1. Crear cuenta y devolver info para correo
        public async Task<(string mensaje, string email, Guid cuentaId)> CrearCuentaAsync(CrearCuentaDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Identificacion == dto.Identificacion))
                return ("Identificación ya registrada.", null, Guid.Empty);

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return ("Correo ya registrado.", null, Guid.Empty);

            if (await _context.Usuarios.AnyAsync(u => u.Numero == dto.Numero))
                return ("Número ya registrado.", null, Guid.Empty);

            var edad = DateTime.Today.Year - dto.FechaNacimiento.Year;
            if (dto.FechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;

            if (edad < 15)
                return ("No se permiten usuarios menores de 15 años.", null, Guid.Empty);

            string hash = Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(dto.Contrasena)));

            var usuario = new Usuario
            {
                Identificacion = dto.Identificacion,
                TipoUsuario = dto.TipoUsuario,
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Email = dto.Email,
                Numero = dto.Numero,
                FechaNacimiento = dto.FechaNacimiento,
                ContrasenaHash = hash
            };


            // Generar número de cuenta único de 10 dígitos
            string numeroCuenta;
            var random = new Random();
            do
            {
                numeroCuenta = random.Next(1000000000, 2000000000).ToString();
            } while (await _context.Cuentas.AnyAsync(c => c.NumeroCuenta == numeroCuenta));

            var cuenta = new Cuenta
            {
                NumeroCuenta = numeroCuenta,
                Saldo = 0,
                FechaCreacion = DateTime.UtcNow,
                Verificada = false,
                Usuario = usuario
            };

            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();

            await _correoService.EnviarVerificacionAsync(usuario.Email, cuenta.NumeroCuenta, cuenta.Id, usuario.Nombres);

            return ("Cuenta creada. Se ha enviado un correo de verificación.", usuario.Email, cuenta.Id);
        }

        // 2. Verificar cuenta
        public async Task<string> VerificarCuentaAsync(Guid cuentaId)
        {
            var cuenta = await _context.Cuentas.Include(c => c.Usuario).FirstOrDefaultAsync(c => c.Id == cuentaId);

            if (cuenta == null)
                return "Cuenta no encontrada.";

            if (cuenta.Verificada)
                return "La cuenta ya está verificada.";

            cuenta.Verificada = true;
            await _context.SaveChangesAsync();

            return "Cuenta verificada.";
        }

        // 3. Obtener cuenta por número
        public async Task<object> ObtenerCuentaPorNumeroAsync(string numero)
        {
            var cuenta = await _context.Cuentas
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numero);

            if (cuenta == null)
                return null;

            return new
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                Saldo = cuenta.Saldo,
                Titular = $"{cuenta.Usuario.Nombres} {cuenta.Usuario.Apellidos}"
            };
        }

        // 4. Eliminar cuenta
        public async Task<string> EliminarCuentaAsync(string numero)
        {
            var cuenta = await _context.Cuentas
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numero);

            if (cuenta == null)
                return "Cuenta no encontrada.";

            if (cuenta.Saldo > 50000)
                return "No se puede eliminar una cuenta con más de $50,000 de saldo.";

            _context.Cuentas.Remove(cuenta);
            await _context.SaveChangesAsync();

            return "Cuenta eliminada.";
        }
    }
}
