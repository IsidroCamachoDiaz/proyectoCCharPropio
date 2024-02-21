using proyectoCCharPropio.DTOS;

namespace proyectoCCharPropio.Servicios
{
    /// <summary>
    /// Interfaz que define métodos para la gestión de incidencias.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public interface interfazTrabajos
    {
        /// <summary>
        /// Crea un nuevo trabajo de incidencia en el sistema.
        /// </summary>
        /// <param name="trabajo">Trabajo de incidencia a crear.</param>
        /// <returns>True si se creó la solicitud correctamente, False en caso contrario.</returns>
        public bool crearTrabajo(TrabajoDTO trabajo);
    }
}
