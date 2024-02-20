using proyectoCCharPropio.DTOS;

namespace proyectoCCharPropio.Servicios
{
    /// <summary>
    /// Interfaz que define métodos para la gestión de tipos de trabajo.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public interface interfazTipos
    {
        /// <summary>
        /// Crea un nuevo tipo de incidencia en el sistema.
        /// </summary>
        /// <param name="tipo">Tipo de incidencia a crear.</param>
        /// <returns>True si se creó la solicitud correctamente, False en caso contrario.</returns>
        public bool CrearTipo(TipoTrabajoDTO tipo);
    }
}
