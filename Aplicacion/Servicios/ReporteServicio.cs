using Entidades;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Persistencia.Contextos;
using System.Text;
using Utilidades;

namespace Aplicacion.Servicios
{
    public class ReporteServicio
    {
        private readonly BancoDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly CorreoService _correoService;

        public ReporteServicio(BancoDbContext context, IWebHostEnvironment env, CorreoService correoService)
        {
            _context = context;
            _env = env;
            _correoService = correoService;
        }

        public async Task<string> GenerarReporteMensualAsync(Usuario usuario, int anio, int mes)
        {
            var movimientos = await _context.Transacciones
                .Include(t => t.CuentaOrigen)
                .Include(t => t.CuentaDestino)
                .Where(t =>
                    (t.CuentaOrigen.UsuarioId == usuario.Id || t.CuentaDestino.UsuarioId == usuario.Id) &&
                    t.Fecha.Year == anio &&
                    t.Fecha.Month == mes)
                .ToListAsync();

            var rutaBase = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "reportes", anio.ToString(), mes.ToString("D2"));
            Directory.CreateDirectory(rutaBase);

            var rutaArchivo = Path.Combine(rutaBase, $"Movimientos-{usuario.Nombres + "-" + usuario.Apellidos}-{mes:D2}-{anio}.pdf");

            var writerProps = new WriterProperties()
                .SetStandardEncryption(
                    Encoding.UTF8.GetBytes(usuario.Identificacion),
                    null,
                    EncryptionConstants.ALLOW_PRINTING,
                    EncryptionConstants.ENCRYPTION_AES_128
                );

            using var writer = new PdfWriter(rutaArchivo, writerProps);
            using var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            // Encabezado
            doc.Add(new Paragraph($"Reporte de Movimientos - {anio}/{mes:D2}")
                .SetFontSize(18)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(10));

            doc.Add(new Paragraph($"Usuario: {usuario.Nombres} ({usuario.Identificacion})")
                .SetFontSize(12)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                .SetMarginBottom(20));

            if (movimientos.Any())
            {
                var tabla = new Table(5).UseAllAvailableWidth();
                tabla.AddHeaderCell("Fecha");
                tabla.AddHeaderCell("Monto");
                tabla.AddHeaderCell("Cuenta Origen");
                tabla.AddHeaderCell("Cuenta Destino");
                tabla.AddHeaderCell("Tipo");

                foreach (var mov in movimientos)
                {
                    tabla.AddCell(mov.Fecha.ToString("dd/MM/yyyy HH:mm"));
                    tabla.AddCell(mov.Monto.ToString("C")); // formato de moneda
                    tabla.AddCell(mov.CuentaOrigen?.NumeroCuenta ?? "N/A");
                    tabla.AddCell(mov.CuentaDestino?.NumeroCuenta ?? "N/A");
                    tabla.AddCell(mov.Tipo);
                }

                doc.Add(tabla);
            }
            else
            {
                doc.Add(new Paragraph("No se encontraron movimientos en este período.")
                    .SetFontSize(14)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
            }

            doc.Add(new Paragraph($"\n\nGenerado automáticamente por Banco NECLI - {DateTime.Now:dd/MM/yyyy HH:mm}")
                .SetFontSize(10)
                .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

            doc.Close();

            await _correoService.EnviarReporteMensualAsync(
                destinatario: usuario.Email,
                cedula: usuario.Identificacion,
                asunto: "Reporte de Movimientos - Mes Anterior",
                cuerpo: $"Hola {usuario.Nombres}, adjunto encontrarás tu reporte de movimientos correspondiente a {mes:D2}/{anio}.",
                rutaPdf: rutaArchivo
            );

            return rutaArchivo;
        }

        public async Task EnviarReportesMensualesAsync(int anio, int mes)
        {
            var usuariosConCuentaVerificada = await _context.Cuentas
                .Include(c => c.Usuario)
                .Where(c => c.Verificada && !string.IsNullOrEmpty(c.Usuario.Email))
                .Select(c => c.Usuario)
                .Distinct()
                .ToListAsync();

            foreach (var usuario in usuariosConCuentaVerificada)
            {
                await GenerarReporteMensualAsync(usuario, anio, mes);
            }
        }

    }
}
