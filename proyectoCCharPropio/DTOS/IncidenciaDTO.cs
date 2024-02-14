using Newtonsoft.Json;

namespace proyectoCCharPropio.DTOS
{
    /// <summary>
    /// Clase que representa una incidencia con sus atributos y métodos asociados.
    /// Esta clase es un Data Transfer Object (DTO) para transferir información de incidencias.
    /// </summary>
    ///  <autor>
    /// Isidro Camacho Diaz
    /// </autor>
    public class IncidenciaDTO
    {
        /// <summary>
        /// Identificador de la incidencia.
        /// </summary>
        [JsonProperty("IdIncidencia")]
        private int IdIncidencia { get; set; }

        /// <summary>
        /// Coste asociado a la incidencia.
        /// </summary>
        [JsonProperty("CosteIncidencia")]
        private float CosteIncidencia { get; set; }

        /// <summary>
        /// Descripción técnica de la incidencia.
        /// </summary>
        [JsonProperty("DescripcionTecnica")]
        private string DescripcionTecnica { get; set; }

        /// <summary>
        /// Descripción proporcionada por el usuario de la incidencia.
        /// </summary>
        [JsonProperty("DescripcionUsuario")]
        private string DescripcionUsuario { get; set; }

        /// <summary>
        /// Estado de la incidencia.
        /// </summary>
        [JsonProperty("EstadoIncidencia")]
        private bool EstadoIncidencia { get; set; }

        /// <summary>
        /// Fecha de fin de la incidencia.
        /// </summary>
        [JsonProperty("FechaFin")]
        private DateTime? FechaFin { get; set; }

        /// <summary>
        /// Fecha de inicio de la incidencia.
        /// </summary>
        [JsonProperty("FechaInicio")]
        private DateTime? FechaInicio { get; set; }

        /// <summary>
        /// Horas dedicadas a la incidencia.
        /// </summary>
        [JsonProperty("HorasIncidencia")]
        private int HorasIncidencia { get; set; }

        /// <summary>
        /// Usuario asociado a la incidencia.
        /// </summary>
        [JsonProperty("IdUsuarioNavigation")]
        private UsuarioDTO IdUsuario;

        /// <summary>
        /// Solicitud asociada a la incidencia.
        /// </summary>
        [JsonProperty("IdSolicitudNavigation")]
        private SolicitudDTO solicitud;

        /// <summary>
        /// Objeto SolicitudDTO asociado a la incidencia.
        /// </summary>
        [JsonIgnore]
        public SolicitudDTO Solicitud { get => solicitud; set => solicitud = value; }

        /// <summary>
        /// Objeto UsuarioDTO asociado a la incidencia.
        /// </summary>
        public UsuarioDTO Usuario { get => IdUsuario; set => IdUsuario = value; }

        /// <summary>
        /// Descripcion del usuario
        /// </summary>
        public string Descripcion_Usuario { get => DescripcionUsuario; set => DescripcionUsuario=value; }





        /// <summary>
        /// Constructor para crear un objeto IncidenciaDTO con algunos atributos.
        /// </summary>
        /// <param name="descripcionUsuario">Descripción proporcionada por el usuario de la incidencia.</param>
        /// <param name="estadoIncidencia">Estado de la incidencia.</param>
        /// <param name="idSolicitud">Objeto SolicitudDTO asociado a la incidencia.</param>
        public IncidenciaDTO(string descripcionUsuario, bool estadoIncidencia, SolicitudDTO idSolicitud)
        {
            DescripcionUsuario = descripcionUsuario;
            EstadoIncidencia = estadoIncidencia;
            Solicitud = idSolicitud;
        }

        /// <summary>
        /// Constructor predeterminado para crear un objeto IncidenciaDTO sin atributos.
        /// </summary>
        public IncidenciaDTO()
        {
        }

        /// <summary>
        /// Constructor para crear un objeto IncidenciaDTO con todos los atributos.
        /// </summary>
        /// <param name="idIncidencia">Identificador de la incidencia.</param>
        /// <param name="costeIncidencia">Coste asociado a la incidencia.</param>
        /// <param name="descripcionTecnica">Descripción técnica de la incidencia.</param>
        /// <param name="descripcionUsuario">Descripción proporcionada por el usuario de la incidencia.</param>
        /// <param name="estadoIncidencia">Estado de la incidencia.</param>
        /// <param name="fechaFin">Fecha de fin de la incidencia.</param>
        /// <param name="fechaInicio">Fecha de inicio de la incidencia.</param>
        /// <param name="horasIncidencia">Horas dedicadas a la incidencia.</param>
        /// <param name="idUsuario">Objeto UsuarioDTO asociado a la incidencia.</param>
        /// <param name="idSolicitud">Objeto SolicitudDTO asociado a la incidencia.</param>
        public IncidenciaDTO(int idIncidencia, float costeIncidencia, string descripcionTecnica, string descripcionUsuario, bool estadoIncidencia, DateTime fechaFin, DateTime fechaInicio, int horasIncidencia, UsuarioDTO idUsuario, SolicitudDTO idSolicitud)
        {
            IdIncidencia = idIncidencia;
            CosteIncidencia = costeIncidencia;
            DescripcionTecnica = descripcionTecnica;
            DescripcionUsuario = descripcionUsuario;
            EstadoIncidencia = estadoIncidencia;
            FechaFin = fechaFin;
            FechaInicio = fechaInicio;
            HorasIncidencia = horasIncidencia;
            Usuario = idUsuario;
            Solicitud = idSolicitud;
        }
    }
}
