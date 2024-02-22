using System.Net.Mail;
using System.Net;

namespace proyectoCCharPropio.Recursos
{
    /// <summary>
    /// Clase para el envio y creacion de correos
    /// </summary>
    ///<autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class Correo
    {
        public Correo()
        {
        }

                /**
        * Método para enviar correo al usuario introducido
        * @param body
        * @param to
        * @param html
        * @param subject
        * @param frommail
        * @param cco
        * @return bool
        */
        public bool EnviarMensaje(string body, string to, bool html, string subject, string frommail, bool cco)
        {
            bool resultado = true;
            SmtpClient smtpClient = null;
            try
            {

                // Parámetros de conexión con un correo de ionos
                string host = "smtp.ionos.es";
                string miLogin = "isidro@isidrocamachodiaz.es";
                string miPassword = "Flash12311oo11oo22oo33oo44";

                // Configurar cliente SMTP
                using (smtpClient = new SmtpClient(host))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(miLogin, miPassword);
                    smtpClient.EnableSsl = true;
                    smtpClient.Port = 587;

                    // Crear mensaje
                    using (MailMessage msg = new MailMessage())
                    {
                        // Dirección de quien lo envía
                        msg.From = new MailAddress($"'SystemRevive' <isidro@isidrocamachodiaz.es>");

                        // A quien envía el correo
                        msg.ReplyToList.Add(new MailAddress(frommail));
                        msg.To.Add(new MailAddress(to));

                        // Si se utiliza copia oculta
                        if (cco)
                            msg.Bcc.Add(new MailAddress(frommail));

                        // Establecer el asunto del mensaje
                        msg.Subject = subject;

                        // Construir el cuerpo
                        if (html)
                        {
                            body = " Restablecer contraseña: " + body;
                            msg.Body = body;
                            msg.IsBodyHtml = true;
                        }
                        else
                        {
                            msg.Body = body;
                            msg.IsBodyHtml = false;
                        }

                        // Enviar el mensaje
                        smtpClient.Send(msg);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR-ImplementacionIntereaccionUsuario-EnviarMensaje] Error: {e.Message}");
                resultado = false;
            }
            finally
            {
                smtpClient?.Dispose();
            }
            return resultado;
        }

        /**
         * Método donde crea el cuerpo del correo
         * @param token
         * @param direccion
         * @return string
         */
        public string MensajeCorreo(string token, string direccion)
        {
            return $@"
<div style=""font-family: 'Optima', sans-serif; max-width: 600px; margin: 0 auto; color: #192255; line-height: 1.6;"">
    <h2 style=""color: #192255; font-size: 24px; font-weight: bold; text-transform: uppercase; margin-bottom: 20px; text-align: left;"">Restablecer Contraseña</h2>

    <p style=""font-size: 16px; text-align: left; margin-bottom: 30px;"">
        Se ha enviado una petición para restablecer la contraseña. Si no has sido tú, por favor cambia la contraseña inmediatamente.
        Si has sido tú, haz clic en el siguiente botón para restablecer tu contraseña:
    </p>

    <div style=""text-align: center;"">
        <a href=""{direccion}?tk={token}"" style=""text-decoration: none;"" target=""_blank"">
            <button style=""background-color: #7d2ae8; color: white; padding: 15px 25px; border: none; border-radius: 5px; font-size: 18px; cursor: pointer; text-transform: uppercase;"">
                Restablecer Contraseña
            </button>
        </a>
    </div>
</div>
";
        }

        /**
         * Método donde crea el cuerpo del correo de alta
         * @param token
         * @param direccion
         * @return string
         */
        public string MensajeCorreoAlta(string token, string direccion)
        {
            return $@"
    <div style=""font-family: 'Optima', sans-serif; max-width: 600px; margin: 0 auto; color: #192255; line-height: 1.6;"">
        <h2 style=""color: #192255; font-size: 24px; font-weight: bold; text-transform: uppercase; margin-bottom: 20px; text-align: left;"">Restablecer Contraseña</h2>

        <p style=""font-size: 16px; text-align: left; margin-bottom: 30px;"">
            Se ha enviado una petición para restablecer la contraseña. Si no has sido tú, por favor cambia la contraseña inmediatamente.
            Si has sido tú, haz clic en el siguiente botón para restablecer tu contraseña:
        </p>

        <div style=""text-align: center;"">
            <a href=""{direccion}?tk={token}"" style=""text-decoration: none;"" target=""_blank"">
                <button style=""background-color: #7d2ae8; color: white; padding: 15px 25px; border: none; border-radius: 5px; font-size: 18px; cursor: pointer; text-transform: uppercase;"">
                    Darme de Alta
                </button>
            </a>
        </div>
    </div>
";
        }
        /**
         * Método donde crea el cuerpo de comfirmacion de alta
         * @param nombreUsuario
         * @return string
         */
        public string MensajeCorreoConfirmacionAlta(string nombreUsuario)
        {
            // Cuerpo del correo en formato HTML
            return @"<div style=""text-align: center; background-color: #3498db; padding: 20px;"">
                <p style=""color: white;"">¡Hola " + nombreUsuario + @", tu cuenta ha sido confirmada por uno de nuestros administradores!</p>
                <p style=""color: white;"">Ya puedes comenzar a utilizar nuestros servicios.</p>
                <p style=""color: white;"">Gracias por ser parte de nuestra plataforma.</p>
            </div>";
        }
        /**
         * Método donde crea el cuerpo del correo de incidencia Terminada
         * @param nombreUsuario
         * @return string
         */
        public string MensajeCorreoConfirmacionTerminada(string nombreUsuario)
        {
            return "<div style=\"text-align: center; background-color: #3498db; padding: 20px;\">\r\n"
                + "    <p style=\"color: white;\">¡Hola " + nombreUsuario + ", tu Incidencia ha sido terminada!</p>\r\n"
                + "    <p style=\"color: white;\">Puedes verificar el estado de tu Incidencia en nuestra plataforma.</p>\r\n"
                + "    <p style=\"color: white;\">Gracias por utilizar nuestros servicios.</p>\r\n"
                + "</div>";
        }



    }
}
