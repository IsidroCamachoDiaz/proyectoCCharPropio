using proyectoCCharPropio.DTOS;

namespace proyectoCCharPropio.Servicios
{
    /// <summary>
    /// Interfaz que define métodos para la gestión de incidencias.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public interface interfazIncidencias
    {
        /// <summary>
        /// Crea una nueva solicitud de incidencia en el sistema.
        /// </summary>
        /// <param name="solicitud">Solicitud de incidencia a crear.</param>
        /// <returns>True si se creó la solicitud correctamente, False en caso contrario.</returns>
        public bool crearSolicitud(SolicitudDTO solicitud);
    }
}
