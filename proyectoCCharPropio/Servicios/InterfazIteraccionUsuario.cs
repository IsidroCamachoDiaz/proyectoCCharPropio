using System;
using proyectoCCharPropio.DTOS;

namespace pruebaRazor.DTOs
{
    /// <summary>
    /// Interfaz que define métodos para la interacción con el usuario.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public interface InterfazIteraccionUsuario
    {
        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="usu">Usuario a registrar.</param>
        /// <returns>True si se registró el usuario correctamente, False en caso contrario.</returns>
        public bool RegistrarUsuario(UsuarioDTO usu);

        /// <summary>
        /// Inicia sesión de un usuario en el sistema.
        /// </summary>
        /// <param name="usu">Usuario que intenta iniciar sesión.</param>
        /// <param name="httpContext">Contexto HTTP asociado a la solicitud.</param>
        /// <returns>Una tarea asincrónica que representa la operación y devuelve True si el usuario inició sesión correctamente, False en caso contrario.</returns>
        public Task<bool> LoginUsuario(UsuarioDTO usu, HttpContext httpContext);

        /// <summary>
        /// Envía un correo electrónico para recuperar la contraseña del usuario.
        /// </summary>
        /// <param name="usu">Usuario del que se desea recuperar la contraseña.</param>
        /// <returns>True si se envió el correo electrónico correctamente, False en caso contrario.</returns>
       public bool RecuperarContrasenya(UsuarioDTO usu);
    }
}
