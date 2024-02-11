using proyectoCCharPropio.DTOS;
using System;

namespace proyectoCCharPropio.Servicios
{
    /// <summary>
    /// Interfaz que define métodos para la administración de usuarios.
    /// </summary>
    ///<autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public interface interfazAdministracion
    {
        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="usu">Usuario a eliminar.</param>
        /// <returns>True si el usuario se eliminó correctamente, False en caso contrario.</returns>
        public bool eliminarUsuario(UsuarioDTO usu);

        /// <summary>
        /// Cambia el acceso de un usuario.
        /// </summary>
        /// <param name="usuario">Usuario al que se le cambiará el acceso.</param>
        /// <param name="idAcceso">Nuevo identificador de acceso.</param>
        /// <returns>True si se cambió el acceso correctamente, False en caso contrario.</returns>
        public bool CambiarAcceso(UsuarioDTO usuario, string idAcceso);

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="usu">Usuario a crear.</param>
        /// <returns>True si se creó el usuario correctamente, False en caso contrario.</returns>
        public bool CrearUsuario(UsuarioDTO usu);
    }
}
