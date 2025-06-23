using Aplicacion.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("reportes")]
[Authorize]
public class ReportesController : ControllerBase
{
    private readonly ReporteServicio _reporteServicio;

    public ReportesController(ReporteServicio reporteServicio)
    {
        _reporteServicio = reporteServicio;
    }

    [HttpPost("enviar-mensuales")]
    public async Task<IActionResult> EnviarReportesMensuales()
    {
        var fecha = DateTime.Now.AddMonths(-1);
        await _reporteServicio.EnviarReportesMensualesAsync(fecha.Year, fecha.Month);
        return Ok("Envio de reportes iniciado.");
    }
}
