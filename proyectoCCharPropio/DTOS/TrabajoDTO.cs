using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    /// <summary>
    /// Clase que representa un trabajo con sus atributos y métodos asociados.
    /// Esta clase es un Data Transfer Object (DTO) para transferir información de trabajos.
    /// </summary>
    /// <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class TrabajoDTO
    {
        /// <summary>
        /// Identificador del trabajo.
        /// </summary>
        [JsonProperty("IdTrabajo")]
        public int IdTrabajo { get; set; }

        /// <summary>
        /// Descripción del trabajo.
        /// </summary>
        [JsonProperty("DescripcionTrabajo")]
        public string DescripcionTrabajo { get; set; } = null!;

        /// <summary>
        /// Estado del trabajo.
        /// </summary>
        [JsonProperty("EstadoTrabajo")]
        public bool EstadoTrabajo { get; set; }

        /// <summary>
        /// Horas del trabajo.
        /// </summary>
        [JsonProperty("HorasTrabajo")]
        public int HorasTrabajo { get; set; }

        /// <summary>
        /// Incidencia asociada al trabajo.
        /// </summary>
        [JsonProperty("IdIncidenciaNavigation")]
        public IncidenciaDTO incidencia{ get; set; }

        /// <summary>
        /// Tipo de trabajo asociado al trabajo.
        /// </summary>
        [JsonProperty("IdTipoIncidenciaNavigation")]
        public TipoTrabajoDTO tipo { get; set; }

        /// <summary>
        /// Constructor predeterminado para crear un objeto TrabajoDTO sin atributos.
        /// </summary>
        public TrabajoDTO()
        {
        }

        /// <summary>
        /// Constructor para crear un objeto TrabajoDTO con algunos atributos.
        /// </summary>
        /// <param name="descripcionTrabajo">Descripción del trabajo.</param>
        /// <param name="estadoTrabajo">Estado del trabajo.</param>
        /// <param name="horasTrabajo">Horas del trabajo.</param>
        /// <param name="incidencia">Incidencia asociada al trabajo.</param>
        /// <param name="tipo">Tipo de trabajo asociado al trabajo.</param>
        public TrabajoDTO(string descripcionTrabajo, bool estadoTrabajo, int horasTrabajo, IncidenciaDTO incidencia, TipoTrabajoDTO tipo)
        {
            DescripcionTrabajo = descripcionTrabajo;
            EstadoTrabajo = estadoTrabajo;
            HorasTrabajo = horasTrabajo;
            this.incidencia = incidencia;
            this.tipo = tipo;
        }

        /// <summary>
        /// Constructor para crear un objeto TrabajoDTO con todos los atributos.
        /// </summary>
        /// <param name="idTrabajo">Identificador del trabajo.</param>
        /// <param name="descripcionTrabajo">Descripción del trabajo.</param>
        /// <param name="estadoTrabajo">Estado del trabajo.</param>
        /// <param name="horasTrabajo">Horas del trabajo.</param>
        /// <param name="incidencia">Incidencia asociada al trabajo.</param>
        /// <param name="tipo">Tipo de trabajo asociado al trabajo.</param>
        public TrabajoDTO(int idTrabajo, string descripcionTrabajo, bool estadoTrabajo, int horasTrabajo, IncidenciaDTO incidencia, TipoTrabajoDTO tipo)
        {
            IdTrabajo = idTrabajo;
            DescripcionTrabajo = descripcionTrabajo;
            EstadoTrabajo = estadoTrabajo;
            HorasTrabajo = horasTrabajo;
            this.incidencia = incidencia;
            this.tipo = tipo;
        }
    }
}
