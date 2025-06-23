using System.Net;
using System.Net.Mail;

namespace Utilidades
{
    public class CorreoService
    {
        private readonly string _smtpServidor = "smtp.gmail.com";
        private readonly int _smtpPuerto = 587;
        private readonly string _remitente = "lbaldovino881@gmail.com";
        private readonly string _clave = "poih aqoe mvvs ekoe";

        public async Task EnviarVerificacionAsync(string destinatario, string numeroCuenta, Guid cuentaId, string nombreUsuario)
        {
            var mensaje = new MailMessage
            {
                From = new MailAddress(_remitente),
                Subject = "Verificación de cuenta",
                IsBodyHtml = true
            };

            mensaje.To.Add(destinatario);


            var html = $@"
                <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; padding: 20px; }}
                            .container {{ background-color: white; padding: 20px; border-radius: 10px; max-width: 600px; margin: auto; }}
                            h2 {{ color: #007BFF; }}
                            .button {{ display: inline-block; margin-top: 20px; padding: 10px 15px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px; }}
                            .footer {{ margin-top: 30px; font-size: 12px; color: #888; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Hola, {nombreUsuario}</h2>
                            <p>Tu cuenta ha sido registrada exitosamente con el siguiente número:</p>
                            <p><strong>Número de cuenta:</strong> {numeroCuenta}</p>
                            <p><strong>Fecha de registro:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}</p>
                            <p>Para verificar tu cuenta, haz clic en el siguiente botón:</p>
                            <a href='https://localhost:5217/cuentas/verificar/{cuentaId}'><button class='button'>Verificar cuenta</button></a>
                            <div class='footer'>
                                Si no solicitaste esta cuenta, puedes ignorar este mensaje.
                            </div>
                        </div>
                    </body>
                </html>";

            mensaje.Body = html;

            using var smtp = new SmtpClient(_smtpServidor, _smtpPuerto)
            {
                Credentials = new NetworkCredential(_remitente, _clave),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mensaje);
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo)
        {
            cuerpo = $@"
                <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; padding: 20px; }}
                            .container {{ background-color: white; padding: 20px; border-radius: 10px; max-width: 600px; margin: auto; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>{asunto}</h2>
                            <p>{cuerpo}</p>
                        </div>
                    </body>
                </html>";

            var mensaje = new MailMessage
            {
                From = new MailAddress(_remitente),
                Subject = asunto,
                Body = cuerpo,
                IsBodyHtml = true
            };

            mensaje.To.Add(destinatario);

            using var smtp = new SmtpClient(_smtpServidor, _smtpPuerto)
            {
                Credentials = new NetworkCredential(_remitente, _clave),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mensaje);
        }

        public async Task EnviarReporteMensualAsync(string destinatario, string cedula, string asunto, string cuerpo, string rutaPdf)
        {
            var mensaje = new MailMessage
            {
                From = new MailAddress(_remitente),
                Subject = asunto,
                Body = $@"
            <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333; padding: 20px; }}
                        .container {{ background-color: white; padding: 20px; border-radius: 10px; max-width: 600px; margin: auto; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>{asunto}</h2>
                        <p>{cuerpo}</p>
                        <p>La contraseña del PDF es tu número de identificación.</p>
                    </div>
                </body>
            </html>",
                IsBodyHtml = true
            };

            mensaje.To.Add(destinatario);

            if (File.Exists(rutaPdf))
            {
                Attachment adjunto = new Attachment(rutaPdf);
                mensaje.Attachments.Add(adjunto);
            }
            else
            {
                throw new FileNotFoundException("El archivo PDF no fue encontrado.", rutaPdf);
            }

            using var smtp = new SmtpClient(_smtpServidor, _smtpPuerto)
            {
                Credentials = new NetworkCredential(_remitente, _clave),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mensaje);
        }


    }
}
